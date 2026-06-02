import traceback

from fastapi import FastAPI, File, HTTPException, UploadFile

from app.services.audio_verification import verify_audio

app = FastAPI()


@app.get("/")
def root():
    return {"message": "Communica AI Verification Service Running"}


@app.post("/verify-audio")
async def verify_audio_endpoint(
    enrolled_audio: UploadFile = File(...),
    sample_audio: UploadFile = File(...),
):
    try:
        enrolled_bytes = await enrolled_audio.read()
        sample_bytes = await sample_audio.read()

        result = await verify_audio(
            enrolled_bytes=enrolled_bytes,
            sample_bytes=sample_bytes,
            enrolled_filename=enrolled_audio.filename,
            sample_filename=sample_audio.filename,
        )
        return result

    except Exception as e:
        print(traceback.format_exc())
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/verify-video")
async def verify_video_endpoint():
    raise HTTPException(
        status_code=501,
        detail="Video verification is not implemented yet. Use audio extracted from the video for now.",
    )