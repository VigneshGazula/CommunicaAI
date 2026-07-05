"""
Video Analysis Service using FastAPI, MediaPipe, and OpenCV
Provides real-time video intelligence metrics for interview analysis
"""

from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from typing import Dict, List
import cv2
import mediapipe as mp
import numpy as np
import base64
from datetime import datetime
import logging

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(title="Video Analysis Service", version="1.0.0")

# CORS configuration
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Initialize MediaPipe
mp_face_mesh = mp.solutions.face_mesh
mp_face_detection = mp.solutions.face_detection

# Response Models
class VideoMetrics(BaseModel):
    timestamp: str
    faceDetected: bool
    eyeContactScore: float
    headPoseScore: float
    smileDetected: bool
    emotionScore: float
    faceVisibility: float
    confidenceScore: float
    
class VideoAnalysisSummary(BaseModel):
    averageEyeContact: float
    averagePosture: float
    averageExpression: float
    videoConfidenceScore: float
    totalFramesAnalyzed: int
    faceDetectionRate: float
    feedback: str


class VideoAnalyzer:
    def __init__(self):
        self.face_mesh = mp_face_mesh.FaceMesh(
            max_num_faces=1,
            refine_landmarks=True,
            min_detection_confidence=0.5,
            min_tracking_confidence=0.5
        )
        self.face_detection = mp_face_detection.FaceDetection(
            model_selection=1,
            min_detection_confidence=0.5
        )
        self.metrics_history: List[VideoMetrics] = []
        
    def analyze_frame(self, frame_data: str) -> VideoMetrics:
        try:
            img_data = base64.b64decode(frame_data.split(',')[1] if ',' in frame_data else frame_data)
            nparr = np.frombuffer(img_data, np.uint8)
            frame = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
            
            if frame is None:
                return self._default_metrics()
            
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            detection_results = self.face_detection.process(rgb_frame)
            face_detected = detection_results.detections is not None and len(detection_results.detections) > 0
            
            if not face_detected:
                return self._default_metrics()
            
            mesh_results = self.face_mesh.process(rgb_frame)
            
            if not mesh_results.multi_face_landmarks:
                return self._default_metrics()
            
            landmarks = mesh_results.multi_face_landmarks[0]
            eye_contact = self._calculate_eye_contact(landmarks, frame.shape)
            head_pose = self._calculate_head_pose(landmarks, frame.shape)
            smile = self._detect_smile(landmarks)
            emotion = self._estimate_emotion(landmarks, smile)
            visibility = self._calculate_visibility(landmarks, frame.shape)
            confidence = self._calculate_confidence(eye_contact, head_pose, visibility)
            
            metrics = VideoMetrics(
                timestamp=datetime.utcnow().isoformat(),
                faceDetected=True,
                eyeContactScore=eye_contact,
                headPoseScore=head_pose,
                smileDetected=smile,
                emotionScore=emotion,
                faceVisibility=visibility,
                confidenceScore=confidence
            )
            
            self.metrics_history.append(metrics)
            return metrics
            
        except Exception as e:
            logger.error(f"Error analyzing frame: {str(e)}")
            return self._default_metrics()
    
    def _calculate_eye_contact(self, landmarks, frame_shape) -> float:
        try:
            left_eye_indices = [33, 133, 160, 159, 158, 157, 173]
            right_eye_indices = [362, 263, 387, 386, 385, 384, 398]
            
            h, w = frame_shape[:2]
            left_eye_x = np.mean([landmarks.landmark[i].x * w for i in left_eye_indices])
            left_eye_y = np.mean([landmarks.landmark[i].y * h for i in left_eye_indices])
            right_eye_x = np.mean([landmarks.landmark[i].x * w for i in right_eye_indices])
            right_eye_y = np.mean([landmarks.landmark[i].y * h for i in right_eye_indices])
            
            eye_alignment = abs(left_eye_y - right_eye_y)
            alignment_score = max(0, 100 - eye_alignment * 2)
            
            face_center_x = (left_eye_x + right_eye_x) / 2
            center_offset = abs(face_center_x - w / 2) / w
            centering_score = max(0, 100 - center_offset * 200)
            
            return min(100, max(0, alignment_score * 0.6 + centering_score * 0.4))
        except:
            return 50.0
    
    def _calculate_head_pose(self, landmarks, frame_shape) -> float:
        try:
            nose_tip = landmarks.landmark[1]
            left_eye = landmarks.landmark[33]
            right_eye = landmarks.landmark[263]
            
            eye_center_y = (left_eye.y + right_eye.y) / 2
            nose_y = nose_tip.y
            vertical_offset = abs(nose_y - eye_center_y - 0.05)
            
            face_center_x = (left_eye.x + right_eye.x) / 2
            horizontal_offset = abs(face_center_x - 0.5)
            
            vertical_score = max(0, 100 - vertical_offset * 500)
            horizontal_score = max(0, 100 - horizontal_offset * 200)
            
            return min(100, max(0, vertical_score * 0.5 + horizontal_score * 0.5))
        except:
            return 70.0
    
    def _detect_smile(self, landmarks) -> bool:
        try:
            left_mouth = landmarks.landmark[61]
            right_mouth = landmarks.landmark[291]
            top_lip = landmarks.landmark[13]
            bottom_lip = landmarks.landmark[14]
            
            mouth_width = abs(right_mouth.x - left_mouth.x)
            mouth_height = abs(top_lip.y - bottom_lip.y)
            ratio = mouth_width / (mouth_height + 0.001)
            
            return ratio > 4.0
        except:
            return False
    
    def _estimate_emotion(self, landmarks, smile_detected: bool) -> float:
        base_score = 60.0
        if smile_detected:
            base_score += 30.0
        return min(100, max(0, base_score))
    
    def _calculate_visibility(self, landmarks, frame_shape) -> float:
        try:
            visible_count = sum(1 for lm in landmarks.landmark if 0 <= lm.x <= 1 and 0 <= lm.y <= 1)
            return min(100, max(0, (visible_count / len(landmarks.landmark)) * 100))
        except:
            return 80.0
    
    def _calculate_confidence(self, eye_contact: float, head_pose: float, visibility: float) -> float:
        return min(100, max(0, eye_contact * 0.4 + head_pose * 0.3 + visibility * 0.3))
    
    def _default_metrics(self) -> VideoMetrics:
        return VideoMetrics(
            timestamp=datetime.utcnow().isoformat(),
            faceDetected=False,
            eyeContactScore=0.0,
            headPoseScore=0.0,
            smileDetected=False,
            emotionScore=0.0,
            faceVisibility=0.0,
            confidenceScore=0.0
        )
    
    def get_summary(self) -> VideoAnalysisSummary:
        if not self.metrics_history:
            return VideoAnalysisSummary(
                averageEyeContact=0.0,
                averagePosture=0.0,
                averageExpression=0.0,
                videoConfidenceScore=0.0,
                totalFramesAnalyzed=0,
                faceDetectionRate=0.0,
                feedback="No video data analyzed"
            )
        
        total_frames = len(self.metrics_history)
        face_detected_frames = sum(1 for m in self.metrics_history if m.faceDetected)
        
        avg_eye_contact = np.mean([m.eyeContactScore for m in self.metrics_history if m.faceDetected] or [0])
        avg_posture = np.mean([m.headPoseScore for m in self.metrics_history if m.faceDetected] or [0])
        avg_expression = np.mean([m.emotionScore for m in self.metrics_history if m.faceDetected] or [0])
        avg_confidence = np.mean([m.confidenceScore for m in self.metrics_history if m.faceDetected] or [0])
        
        detection_rate = (face_detected_frames / total_frames) * 100 if total_frames > 0 else 0
        feedback = self._generate_feedback(avg_eye_contact, avg_posture, avg_expression, detection_rate)
        
        return VideoAnalysisSummary(
            averageEyeContact=round(avg_eye_contact, 2),
            averagePosture=round(avg_posture, 2),
            averageExpression=round(avg_expression, 2),
            videoConfidenceScore=round(avg_confidence, 2),
            totalFramesAnalyzed=total_frames,
            faceDetectionRate=round(detection_rate, 2),
            feedback=feedback
        )
    
    def _generate_feedback(self, eye_contact: float, posture: float, expression: float, detection_rate: float) -> str:
        feedback_parts = []
        if detection_rate < 80:
            feedback_parts.append("Ensure your face is clearly visible to the camera")
        if eye_contact < 60:
            feedback_parts.append("Maintain better eye contact with the camera")
        elif eye_contact >= 80:
            feedback_parts.append("Excellent eye contact maintained")
        if posture < 60:
            feedback_parts.append("Keep your head centered and maintain good posture")
        elif posture >= 80:
            feedback_parts.append("Great posture throughout the interview")
        if expression < 50:
            feedback_parts.append("Try to appear more engaged and positive")
        elif expression >= 70:
            feedback_parts.append("Positive and engaging facial expressions")
        return "; ".join(feedback_parts) if feedback_parts else "Good overall video presence"
    
    def reset(self):
        self.metrics_history = []


analyzer = VideoAnalyzer()


@app.get("/")
def read_root():
    return {"service": "Video Analysis Service", "status": "running", "version": "1.0.0"}


@app.get("/health")
def health_check():
    return {"status": "healthy"}


@app.post("/analyze-frame")
async def analyze_frame(frame_data: Dict[str, str]):
    try:
        if "frame" not in frame_data:
            raise HTTPException(status_code=400, detail="Missing 'frame' in request body")
        metrics = analyzer.analyze_frame(frame_data["frame"])
        return metrics
    except Exception as e:
        logger.error(f"Error in analyze_frame: {str(e)}")
        raise HTTPException(status_code=500, detail=str(e))


@app.get("/summary")
def get_summary():
    try:
        return analyzer.get_summary()
    except Exception as e:
        logger.error(f"Error in get_summary: {str(e)}")
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/reset")
def reset_analyzer():
    try:
        analyzer.reset()
        return {"status": "reset", "message": "Analyzer reset successfully"}
    except Exception as e:
        logger.error(f"Error in reset: {str(e)}")
        raise HTTPException(status_code=500, detail=str(e))


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8001, log_level="info")
