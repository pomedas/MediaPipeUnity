# How to run mediapipe scripts

1. Create Python Virtual environment
```
python3 -m venv mp_env
source mp_env/bin/activate
```
2. Install Mediapie using pip
```
pip install mediapipe
```

In case of using an Apple M1 processor (Mediape is not supported officially yet)

- Make sure you have the latest version of pip and to the latest version (22.1.2)
```
python3 -m pip install --upgrade pip
```
- Install requirements and the mediapipe package for M1 processor
```
pip install protobuf==3.20.1
pip install mediapipe-silicon
```
3. execute the python script
```
python3 pose.py
```
or 
```
python3 hands.py
```
4. To deactivate the virtual environment
```
deactivate
```
