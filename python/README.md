#README
How to run mediapipe scripts

#Create Python Virtual environment
python3 -m venv mp_env
source mp_env/bin/activate

#Install Mediapie using pip
pip install mediapipe

#On Apple M1 processor
## Upgrade pip to the latest version (22.1.2)
python3 -m pip install --upgrade pip
pip install protobuf==3.20.1
pip install mediapipe-silicon

#execute the python
python3 pose.py

#To deactivate the virtual environment
deactivate
