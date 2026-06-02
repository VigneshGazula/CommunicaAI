from pathlib import Path
from tempfile import NamedTemporaryFile
from typing import Optional

import torchaudio
from speechbrain.inference.speaker import SpeakerRecognition

verification = SpeakerRecognition.from_hparams(
    source="speechbrain/spkrec-ecapa-voxceleb"
)


def _suffix_from_filename(filename: Optional[str]) -> str:
    suffix = Path(filename or "").suffix
    return suffix if suffix else ".wav"


def _write_temp_audio(data: bytes, suffix: str) -> Path:
    temp_file = NamedTemporaryFile(delete=False, suffix=suffix)
    try:
        temp_file.write(data)
        temp_file.flush()
        return Path(temp_file.name)
    finally:
        temp_file.close()


def _load_audio_tensor(path: Path):
    waveform, sample_rate = torchaudio.load(str(path))

    if waveform.shape[0] > 1:
        waveform = waveform.mean(dim=0, keepdim=True)

    if sample_rate != 16000:
        waveform = torchaudio.functional.resample(waveform, sample_rate, 16000)

    return waveform


async def verify_audio(
    enrolled_bytes: bytes,
    sample_bytes: bytes,
    enrolled_filename: Optional[str] = None,
    sample_filename: Optional[str] = None,
):
    enrolled_path = _write_temp_audio(
        enrolled_bytes,
        _suffix_from_filename(enrolled_filename),
    )
    sample_path = _write_temp_audio(
        sample_bytes,
        _suffix_from_filename(sample_filename),
    )

    try:
        enrolled_waveform = _load_audio_tensor(enrolled_path)
        sample_waveform = _load_audio_tensor(sample_path)

        score, prediction = verification.verify_batch(
            enrolled_waveform,
            sample_waveform,
        )

        return {
            "verified": bool(prediction.squeeze().item()),
            "score": float(score.squeeze().item()),
        }
    finally:
        if enrolled_path.exists():
            enrolled_path.unlink()
        if sample_path.exists():
            sample_path.unlink()