from fastapi import FastAPI, UploadFile, File
from app.services.audio_verification import verify_audio

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Communica AI Verification Service Running"}

@app.post("/verify-audio")
async def verify_audio_endpoint(
    enrolled_audio: UploadFile = File(...),
    sample_audio: UploadFile = File(...)
):
    enrolled_bytes = await enrolled_audio.read()
    sample_bytes = await sample_audio.read()

    result = await verify_audio(enrolled_bytes, sample_bytes)
    return result

@app.post("/verify-video")
async def verify_video_endpoint():
    return {"verified": True, "message": "Video endpoint placeholder"}