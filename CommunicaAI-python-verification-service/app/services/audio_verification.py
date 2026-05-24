import tempfile
from scipy.spatial.distance import cosine
from speechbrain.inference.speaker import SpeakerRecognition


verification = SpeakerRecognition.from_hparams(
    source="speechbrain/spkrec-ecapa-voxceleb"
)


async def verify_audio(enrolled_bytes: bytes, sample_bytes: bytes):
    with tempfile.NamedTemporaryFile(suffix=".wav") as enrolled_temp:
        with tempfile.NamedTemporaryFile(suffix=".wav") as sample_temp:

            enrolled_temp.write(enrolled_bytes)
            sample_temp.write(sample_bytes)

            enrolled_temp.flush()
            sample_temp.flush()

            enrolled_embedding = verification.encode_file(
                enrolled_temp.name
            )

            sample_embedding = verification.encode_file(
                sample_temp.name
            )

            similarity = 1 - cosine(
                enrolled_embedding.squeeze().detach().numpy(),
                sample_embedding.squeeze().detach().numpy()
            )

            threshold = 0.75

            verified = similarity >= threshold

            return {
                "verified": bool(verified),
                "score": float(similarity)
            }