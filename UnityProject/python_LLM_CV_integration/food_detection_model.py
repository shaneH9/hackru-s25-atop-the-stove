import tensorflow as tf
from tensorflow.keras import layers
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras.applications import MobileNetV2

import numpy as np
import os
import cv2
import matplotlib.pyplot as plt

# dependencies: tensorflow, numpy, os, cv2, matplotlib