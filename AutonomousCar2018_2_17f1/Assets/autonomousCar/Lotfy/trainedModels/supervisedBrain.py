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


# [left,right,foreward]
modelName=''
modelPath='/Users/MohamedAshraf/Desktop/trainedModels/'+modelName+'.h5'
sharedMemoryPath='/Users/MohamedAshraf/Desktop/trainedModels/sharedMemory/'
model=load_model(modelPath)

def socketHandShaking(s,IP,Port):
    print('socket created ')
    s.bind((IP, Port)) #bind port with ip address
    print('socket binded to port ')
    s.listen(5)#listening for connection
    print('socket listensing ... ')
    
def getCurrentAction(imageID):
    img=Image.open(sharedMemoryPath+str(imageID)+'.png')
    img=img.resize((128,128))
    print(str(imageID)+'.png')
    plt.imshow(img)
    plt.show()
    return str(model.predict(np.array(img).reshape(-1, 128, 128, 3)).argmax())


def sending_and_reciveing():
    imageID=0
    s = socket.socket()
    socketHandShaking(s,'127.0.0.1',1126)
    while True:
        c, addr = s.accept() #when port connected
        try:
            UpdateValue=getCurrentAction(imageID)
            print(UpdateValue)
            imageID = imageID+1
        except:
            UpdateValue=str(3)
        c.sendall(UpdateValue[0].encode("utf-8"))#then encode and send taht string back to unity
        c.close()

sending_and_reciveing()#calling the function to run server