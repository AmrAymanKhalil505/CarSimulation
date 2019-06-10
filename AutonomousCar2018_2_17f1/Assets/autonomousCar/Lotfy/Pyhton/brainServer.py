import socket
import os
import numpy as np
import pandas as pd 
import matplotlib.pyplot as plt
from PIL import Image
from keras.layers import *
from keras.optimizers import *
from keras.models import load_model
from keras.models import Sequential
from keras.preprocessing.image import ImageDataGenerator


# [left,right,foreward]
modelName = 'Maneuvering1'
socketNumber = 1200
modelPath = '/Users/MohamedAshraf/Desktop/'+modelName+'.h5'
sharedMemoryPath = '/Users/MohamedAshraf/Desktop/sharedMemory_agent#1/'
model = load_model(modelPath)

def TCPHandShaking(s,IP,Port):
    print('socket created ')
    s.bind((IP, Port)) #bind port with ip address
    print('socket binded to port ')
    s.listen(5) #listening for connection
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
    TCPHandShaking(s,'127.0.0.1',socketNumber)
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

sending_and_reciveing()