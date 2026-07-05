# Video Analysis Service

Real-time video intelligence service for interview analysis using FastAPI, MediaPipe, and OpenCV.

## Features

- Face Detection
- Eye Contact Detection
- Head Pose Estimation
- Smile Detection
- Basic Emotion Detection
- Face Visibility Detection
- Real-time metrics streaming

## Installation

```bash
# Install dependencies
pip install -r requirements.txt
```

## Running the Service

```bash
# Development
python main.py

# Production
uvicorn main:app --host 0.0.0.0 --port 8001
```

## API Endpoints

### GET /
Service information

### GET /health
Health check

### POST /analyze-frame
Analyze a single video frame
```json
{
  "frame": "base64_encoded_image_data"
}
```

### GET /summary
Get analysis summary of all processed frames

### POST /reset
Reset the analyzer for a new session

## Integration with ASP.NET Backend

The ASP.NET backend should call this service via HTTP requests to analyze video frames during interviews.

## Requirements

- Python 3.8+
- FastAPI
- OpenCV
- MediaPipe
- NumPy
