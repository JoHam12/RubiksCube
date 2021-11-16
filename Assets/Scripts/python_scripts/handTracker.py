import cv2
import mediapipe as mp
import time

def GetDistance(x, y):
    return (x**2+y**2)**.5

MULTIPLIER = 100
CLOSEVALUE = 12

cam = cv2.VideoCapture(0, cv2.CAP_DSHOW)
cam.set(cv2.CAP_PROP_FPS, 30)

mpHands = mp.solutions.hands
hands = mpHands.Hands(static_image_mode=False,
                    max_num_hands=1,
                    min_detection_confidence=0.5,
                    min_tracking_confidence=0.5)
mpDraw = mp.solutions.drawing_utils

pTime = 0
cTime = 0

while True:
    success, img = cam.read()
    imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
    results = hands.process(imgRGB)
    #print(positionX + ";" + positionY)
   
    if results.multi_hand_landmarks:
        for handLms in results.multi_hand_landmarks:
            print(handLms.landmark)
            for id, lm in enumerate(handLms.landmark):
                h, w, c = img.shape
                #print(h, w, c)
                cx, cy = int(lm.x *w), int(lm.y*h)
                #if id ==0:
                cv2.circle(img, (cx,cy), 3, (255,0,255), cv2.FILLED)
                #print(img)
            mpDraw.draw_landmarks(img, handLms, mpHands.HAND_CONNECTIONS)
            # print(mpHands.HAND_CONNECTIONS)
            #print(handLms)
            dist1 = (GetDistance(handLms.landmark[8].x - handLms.landmark[5].x, handLms.landmark[8].y - handLms.landmark[5].y) * MULTIPLIER)**2
            dist2 = (GetDistance(handLms.landmark[12].x - handLms.landmark[9].x, handLms.landmark[12].y - handLms.landmark[9].y) * MULTIPLIER)**2
            dist3 = (GetDistance(handLms.landmark[16].x - handLms.landmark[13].x, handLms.landmark[16].y - handLms.landmark[13].y) * MULTIPLIER)**2
            dist4 = (GetDistance(handLms.landmark[20].x - handLms.landmark[17].x, handLms.landmark[20].y - handLms.landmark[17].y) * MULTIPLIER)**2
            cond = dist1 <= CLOSEVALUE and dist2 <= CLOSEVALUE and dist3 <= CLOSEVALUE and dist4 <= CLOSEVALUE
            oneFingerCond = dist2 <= CLOSEVALUE and dist3 <= CLOSEVALUE and dist4 <= CLOSEVALUE and dist1 > CLOSEVALUE
            isOpen = not cond
            with open("python_scripts\\test.txt", 'a', encoding='utf-8') as f:
                if results.multi_hand_landmarks[-1].landmark[9].x >= .7:
                    positionX = "-1"
                elif results.multi_hand_landmarks[-1].landmark[9].x <= .3:
                    positionX = "1"
                else:
                    positionX = "0"
                if results.multi_hand_landmarks[-1].landmark[9].y <= .3:
                    positionY = "1"
                elif results.multi_hand_landmarks[-1].landmark[9].y >= .7:
                    positionY = "-1"
                else:
                    positionY = "0"
                f.write("X:" + positionX + ";Y:" + positionY + ";Open:" + str(int(isOpen)))
                f.write(";XHand:" + str(results.multi_hand_landmarks[-1].landmark[9].x) + ";YHand:" + str(results.multi_hand_landmarks[-1].landmark[9].y))
                f.write(";Select:" + str(int(oneFingerCond)) + ";\n")
    else:
        with open("python_scripts\\test.txt", 'a', encoding='utf-8') as f:
            f.write("X:0;Y:0;Open:0;XHand:0;YHand:0;Select:0;\n")
            
    cTime = time.time()
    fps = 1/(cTime-pTime)
    pTime = cTime

    cv2.putText(img,str(int(fps)), (10,70), cv2.FONT_HERSHEY_PLAIN, 3, (255,0,255), 3)

    cv2.imshow("Image", img)

    with open("python_scripts\\breakFile.txt", 'r') as f:
        if("break" in f.read()):
            break
    if cv2.waitKey(1) & 0xff == ord('q'):
        break


    