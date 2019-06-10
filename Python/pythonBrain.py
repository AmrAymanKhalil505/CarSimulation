# Names: Mohamed Lotfy, Loai Alaa, Mohab Tarek
import socket
import os
from PIL import Image
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd 
import cv2
from keras.layers import *
from keras.optimizers import *
from keras.models import Sequential
from keras.models import load_model
from keras.preprocessing.image import ImageDataGenerator

# variables
modelName = 'maneuveringModel'
socketNumber=1200
print(socketNumber)
modelPath='/Users/MohamedAshraf/Desktop/'+modelName+'.h5'
sharedMemoryPath='/Users/MohamedAshraf/Desktop/sharedMemory_agent#1/'
model=load_model(modelPath)

# function 
# responsible for the socket initial handshake between this pythin server and the unity TCP client
def socketHandShaking(sckt,IP,Port):
    print('socket created ')
    sckt.bind((IP, Port)) 			#bind port with ip address
    print('socket binded to port ')
    sckt.listen(5)					#listening for connection
    print('socket listensing ... ')
    

# function
# given an image ID this function would load this image from the shared directory and make the prediction on this image using the trained model
# this function will return the model prediction made by the trained model
def makePrediction(imageID):
    img=Image.open(sharedMemoryPath+str(imageID)+'.png')
    img=img.resize((128,128))
    print(str(imageID)+'.png')
    plt.imshow(img)
    plt.show()
    return str(model.predict(np.array(img).reshape(-1, 128, 128, 3)).argmax())


# function
# this function run over and over to load each new image written in the shared memory and make a prediction using this 
# image and send this prediction back into Unity via the TCP connection
def send_prediction():
    imageID=0
    s = socket.socket()
    socketHandShaking(s,'127.0.0.1',socketNumber)
    while True:
        c, addr = s.accept() 	#when port connected
        try:
            UpdateValue=makePrediction(imageID)
            print(UpdateValue)
            imageID = imageID+1
        except:
            UpdateValue=str(3)
        c.sendall(UpdateValue[0].encode("utf-8"))	#then encode and send taht string back to unity
        c.close()

send_prediction()	#calling the function to run server