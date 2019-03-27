#         ---------READ ME FIRST--------
# To enable the capabilty of python controlling the car over the TCP connection
#  Step 1 : run this script as its the server
#  Step 2 : uncheck the car user controller script in the inspector of the car in the map gen scene in lotfy's folder
#  Step 3 : check the pythonRecievedActions script in the inspector in the same place as step 2
#  Step 4 : run unity and the car will move in one of 4 directions that you decide for now as we dont have the array of actions yet 





import  socket
def sending_and_reciveing():
    s = socket.socket()
    print('socket created ')
    port = 1234
    s.bind(('127.0.0.1', port)) #bind port with ip address
    print('socket binded to port ')
    s.listen(5)#listening for connection
    print('socket listensing ... ')
    while True:
        c, addr = s.accept() #when port connected
        print("\ngot connection from ", addr)
        de=c.recv(1024).decode("utf-8") #Collect data from port and decode into  string
        print('Getting Data from the Unity : ',de)
        UpdateValue=de.split()
        print('After changing data sending back to Unity')
        print(UpdateValue)
        print("to be sent back: "+UpdateValue[0])
        c.sendall(UpdateValue[0].encode("utf-8"))#then encode and send taht string back to unity
        c.close()

sending_and_reciveing()#calling the function to run server