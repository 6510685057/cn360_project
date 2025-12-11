# import cv2
# import time
# import os

# # ใช้ haarcascade ที่มากับ OpenCV เลย ไม่ต้องโหลดเพิ่ม
# face_cascade = cv2.CascadeClassifier(
#     cv2.data.haarcascades + "haarcascade_frontalface_default.xml"
# )

# # เปิดกล้องตัวแรก (ถ้ามีกล้องหลายตัวอาจต้องเปลี่ยนเป็น 1,2)
# cap = cv2.VideoCapture(0)

# # ตั้งชื่อไฟล์ state (ให้ไปอยู่โฟลเดอร์เดียวกับสคริปต์นี้)
# ATTENTION_FILE = "attention_state.txt"

# last_state = "ATTENTION"
# last_write_time = 0

# while True:
#     ret, frame = cap.read()
#     if not ret:
#         print("ไม่เจอภาพจากกล้องเลยนะ")
#         break

#     gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

#     # ตรวจจับใบหน้า
#     faces = face_cascade.detectMultiScale(
#         gray,
#         scaleFactor=1.3,
#         minNeighbors=5,
#         minSize=(80, 80)
#     )

#     # ตัดสินใจสถานะ
#     if len(faces) > 0:
#         state = "ATTENTION"
#     else:
#         state = "NO_ATTENTION"

#     # วาดกรอบให้เราดูเล่น ๆ
#     for (x, y, w, h) in faces:
#         cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)

#     cv2.putText(frame, state, (10, 30),
#                 cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)

#     cv2.imshow("Reaction Trainer Camera", frame)

#     # เขียนไฟล์เฉพาะตอน state เปลี่ยน หรือทุก ๆ 0.5 วิ
#     now = time.time()
#     if state != last_state or now - last_write_time > 0.5:
#         with open(ATTENTION_FILE, "w") as f:
#             f.write(state)
#         last_state = state
#         last_write_time = now
#         # print("Write state:", state)

#     # กด q เพื่อปิดหน้าต่างกล้อง
#     if cv2.waitKey(1) & 0xFF == ord('q'):
#         break

# cap.release()
# cv2.destroyAllWindows()
import cv2
import time
import sys
import json

# โหลด Haar Cascade สำหรับตรวจจับหน้า
face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")

cam = cv2.VideoCapture(0)

if not cam.isOpened():
    print("ERROR_NO_CAMERA")
    sys.stdout.flush()
    exit()

face_missing = False
missing_start = 0

while True:
    ret, frame = cam.read()
    if not ret:
        print("ERROR_READ")
        sys.stdout.flush()
        break

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    faces = face_cascade.detectMultiScale(gray, 1.3, 5)

    if len(faces) == 0:
        if not face_missing:
            face_missing = True
            missing_start = time.time()
        else:
            if time.time() - missing_start > 1.0:  # หายไป 1 วิ
                print("NO_FACE")
                sys.stdout.flush()
    else:
        if face_missing:
            print("FACE_OK")
            sys.stdout.flush()
        face_missing = False

    time.sleep(0.1)
