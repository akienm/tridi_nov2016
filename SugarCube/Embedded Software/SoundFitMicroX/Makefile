#
# Copyright 2011 Alan Burlison, alan@bleaklow.com.  All rights reserved.
# Use is subject to license terms.
#
# Redistribution and use in source and binary forms, with or without
# modification, are permitted provided that the following conditions are met:
#
#  1. Redistributions of source code must retain the above copyright notice,
#     this list of conditions and the following disclaimer.
#
#  2. Redistributions in binary form must reproduce the above copyright notice,
#     this list of conditions and the following disclaimer in the documentation
#     and/or other materials provided with the distribution.
#
# THIS SOFTWARE IS PROVIDED BY ALAN BURLISON "AS IS" AND ANY EXPRESS OR IMPLIED
# WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
# MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
# EVENT SHALL ALAN BURLISON OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
# INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
# LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
# OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
# LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
# NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
# EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#
# Makefile for building Arduino projects outside of the Arduino environment
#
# This makefile should be included into a per-project Makefile of the following
# form:
#
# ----------

BOARD = uno

# Edit this, or pass in something else like "make upload PORT=/dev/cu.whatever"
#
# One advantage to defining or passing in the PORT is that you don't
# have to kill your connection to the SugarCube just to find out what
# port it's on
#
#PORT?= /dev/cu.usbmodem1411
PORT?= $(shell ./flashCube.py -q)  # quiet
#$(warning SugarCube on $(PORT).)

RAW_SVNVERSION = $(shell svnversion)
XSVNVERSION = $(subst :,_,$(RAW_SVNVERSION))  # Make barfs on targets with colons
SVNVERSION = $(strip $(XSVNVERSION))
#$(warning Building svn version $(SVNVERSION).)

# INC_DIRS = ../common
INC_DIRS = $(ARD_HOME)/libraries/EEPROM
LIB_HOME = $(HOME)/Documents/Arduino/libraries
LIB_DIRS = $(ARD_HOME)/libraries/EEPROM $(LIB_HOME)/AccelStepper $(LIB_HOME)/AFMotor
# include ../../Makefile.master
# ----------
#
# Where:
#   BOARD    : Arduino board type, from $(ARD_HOME)/hardware/boards.txt
#   PORT     : USB port
#   INC_DIRS : List of directories containing header files
#   LIB_DIRS : List of directories containing library source
#
# Before using this Makefile you can adjust the following macros to suit
# your environment, either by editing this file directly or by defining them in
# the Makefile that includes this one, in which case they will override the
# definitions below:
#   ARD_REV      : arduino software revision, e.g. 0017, 0018
#   ARD_HOME     : installation directory of the Arduino software.
#   ARD_BIN      : location of compiler binaries
#   AVRDUDE      : location of avrdude executable
#   AVRDUDE_CONF : location of avrdude configuration file
#   PROGRAMMER   : avrdude programmer type
#   MON_SPEED    : serial monitor speed
#   EXTRA_FLAGS  : any extra flags that should be passed to the compilers
#

# Global configuration.
ARD_REV ?= 105
ARD_HOME ?= /Applications/Arduino.app//Contents/Resources/Java
ARD_BIN ?= $(ARD_HOME)/hardware/tools/avr/bin
AVRDUDE ?= $(ARD_BIN)/avrdude
AVRDUDE_CONF ?= $(ARD_HOME)/hardware/tools/avr/etc/avrdude.conf
PROGRAMMER ?= arduino
MON_SPEED ?= 9600

### Nothing below here should require editing. ###

# Check for the required definitions.
ifndef BOARD
    $(error $$(BOARD) not defined)
endif
ifndef PORT
    $(error $$(PORT) not defined)
endif

# Version-specific settings
ifeq "$(ARD_REV)" "0017"
    ARD_BOARDS = $(ARD_HOME)/hardware/boards.txt
    ARD_SRC_DIR = $(ARD_HOME)/hardware/cores/arduino
    ARD_MAIN = $(ARD_SRC_DIR)/main.cxx
else
    ARD_BOARDS = $(ARD_HOME)/hardware/arduino/boards.txt
    ARD_SRC_DIR = $(ARD_HOME)/hardware/arduino/cores/arduino
    ARD_VARIANT_DIR = $(ARD_HOME)/hardware/arduino/variants/standard
    ARD_MAIN = $(ARD_SRC_DIR)/main.cpp
endif

# Platform-specific settings.
PLATFORM = $(shell uname -s)
ifeq "$(PLATFORM)" "SunOS"
    define run-monitor
	gnome-terminal -t '$(BOARD) $(PORT)' \
	    -e 'env -i tip -$(MON_SPEED) $(PORT)' &
    endef
    define kill-monitor
	- pkill -f 'tip.*$(PORT)'
    endef
else ifeq "$(PLATFORM)" "Darwin"  # Was and also works on Linux
    define run-monitor
	miniterm.py $(PORT) $(MON_SPEED)
    endef
    define kill-monitor
	- pkill -f 'miniterm.py.*$(PORT)'
    endef
#else ifeq "$(PLATFORM)" "Darwin"
#    $(error No monitor command for platform $(PLATFORM))
else
    $(error Unknown platform $(PLATFORM))
endif

# Standard macros.
SKETCH = $(notdir $(CURDIR))
BUILD_DIR = build
VPATH = $(LIB_DIRS)

# Macros derived from boards.txt
MCU := $(shell sed -n 's/$(BOARD)\.build\.mcu=\(.*\)/\1/p' < $(ARD_BOARDS))
F_CPU := $(shell sed -n 's/$(BOARD)\.build\.f_cpu=\(.*\)/\1/p' < $(ARD_BOARDS))
UPLOAD_SPEED := \
    $(shell sed -n 's/$(BOARD)\.upload\.speed=\(.*\)/\1/p' < $(ARD_BOARDS))

# Build tools.
CC = $(ARD_BIN)/avr-gcc
CXX = $(ARD_BIN)/avr-g++
CXXFILT = $(ARD_BIN)/avr-c++filt
OBJCOPY = $(ARD_BIN)/avr-objcopy
OBJDUMP = $(ARD_BIN)/avr-objdump
AR = $(ARD_BIN)/avr-ar
SIZE = $(ARD_BIN)/avr-size
NM = $(ARD_BIN)/avr-nm
MKDIR = mkdir -p
RM = rm -rf
MV = mv -f

# Compiler flags.
INC_FLAGS = \
    $(addprefix -I,$(INC_DIRS)) $(addprefix -I,$(LIB_DIRS)) -I$(ARD_SRC_DIR) -I$(ARD_VARIANT_DIR)
ARD_FLAGS = -mmcu=$(MCU) -DF_CPU=$(F_CPU) -DARDUINO=$(ARD_REV)
C_CXX_FLAGS = \
    -Wall -Wextra -Wundef -Wno-unused-parameter \
    -fdiagnostics-show-option -g -Wa,-adhlns=$(BUILD_DIR)/$*.lst \
    $(EXTRA_FLAGS)
C_FLAGS = \
    -std=gnu99 -Wstrict-prototypes -Wno-old-style-declaration $(C_CXX_FLAGS)
CXX_FLAGS = \
    $(C_CXX_FLAGS)

# Optimiser flags.
#     optimise for size, unsigned by default, pack data.
#     separate sections, drop unused ones, shorten branches, jumps.
#     don't inline, vectorise loops. no exceptions.
#     no os preamble, use function calls in prologues.
# http://gcc.gnu.org/onlinedocs/gcc-4.3.5/gcc/
# http://www.tty1.net/blog/2008-04-29-avr-gcc-optimisations_en.html
OPT_FLAGS = \
     -Os -funsigned-char -funsigned-bitfields -fpack-struct -fshort-enums \
    -ffunction-sections -fdata-sections -Wl,--gc-sections,--relax \
    -fno-inline-small-functions -fno-tree-scev-cprop -fno-exceptions \
    -ffreestanding -mcall-prologues

# Build parameters.
IMAGE = $(BUILD_DIR)/$(SKETCH)
VERSIONED_HEX_IMAGE = $(SKETCH)_$(SVNVERSION)
ARD_C_SRC = $(wildcard $(ARD_SRC_DIR)/*.c)
ARD_CXX_SRC = $(wildcard $(ARD_SRC_DIR)/*.cpp)
ARD_C_OBJ = $(patsubst %.c,%.o,$(notdir $(ARD_C_SRC)))
ARD_CXX_OBJ = $(patsubst %.cpp,%.o,$(notdir $(ARD_CXX_SRC)))
ARD_LIB = arduino
ARD_AR = $(BUILD_DIR)/lib$(ARD_LIB).a
ARD_AR_OBJ = $(ARD_AR)($(ARD_C_OBJ) $(ARD_CXX_OBJ))
ARD_LD_FLAG = -l$(ARD_LIB)

# Workaround for http://gcc.gnu.org/bugzilla/show_bug.cgi?id=34734
$(ARD_AR)(Tone.o) : CXX_FLAGS += -w

# Sketch libraries.
LIB_C_SRC = $(foreach ld,$(LIB_DIRS),$(wildcard $(ld)/*.c))
LIB_CXX_SRC = $(foreach ld,$(LIB_DIRS),$(wildcard $(ld)/*.cpp))
LIB_SRC = $(LIB_C_SRC) $(LIB_CXX_SRC)
ifneq "$(strip $(LIB_C_SRC) $(LIB_CXX_SRC))" ""
	LIB_C_OBJ = $(patsubst %.c,%.o,$(notdir $(LIB_C_SRC)))
	LIB_CXX_OBJ = $(patsubst %.cpp,%.o,$(notdir $(LIB_CXX_SRC)))
	LIB_LIB = library
	LIB_AR = $(BUILD_DIR)/lib$(LIB_LIB).a
	LIB_AR_OBJ = $(LIB_AR)($(LIB_C_OBJ) $(LIB_CXX_OBJ))
	LIB_LD_FLAG = -l$(LIB_LIB)
endif

# Sketch INO source.
SKT_INO_SRC = $(wildcard *.ino)
ifneq "$(strip $(SKT_INO_SRC))" ""
	SKT_INO_OBJ = $(BUILD_DIR)/$(SKETCH)_ino.o
endif

# C and C++ source.
SKT_C_SRC = $(wildcard *.c)
SKT_CXX_SRC = $(wildcard *.cpp)
ifneq "$(strip $(SKT_C_SRC) $(SKT_CXX_SRC))" ""
	SKT_C_OBJ = $(patsubst %.c,%.o,$(SKT_C_SRC))
	SKT_CXX_OBJ = $(patsubst %.cpp,%.o,$(SKT_CXX_SRC))
	SKT_LIB = sketch
	SKT_AR = $(BUILD_DIR)/lib$(SKT_LIB).a
	SKT_AR_OBJ = $(SKT_AR)/($(SKT_C_OBJ) $(SKT_CXX_OBJ))
	SKT_LD_FLAG = -l$(SKT_LIB)
endif

# Common rule bodies.
define run-cc
	$(CC) -c $(C_FLAGS) $(OPT_FLAGS) $(ARD_FLAGS) $(INC_FLAGS) \
	    -MD -MT '$@($%)' -MF $(@D)/.$(@F)_$*.dep $< -o $(BUILD_DIR)/$%
	@ $(AR) rc $@ $(BUILD_DIR)/$%
	@ $(RM) $(BUILD_DIR)/$%
	@ $(CXXFILT) < $(BUILD_DIR)/$*.lst > $(BUILD_DIR)/$*.lst.tmp
	@ $(MV) $(BUILD_DIR)/$*.lst.tmp $(BUILD_DIR)/$*.lst
endef

define run-cxx
	$(CXX) -c $(CXX_FLAGS) $(OPT_FLAGS) $(ARD_FLAGS) $(INC_FLAGS) \
	    -MD -MT '$@($%)' -MF $(@D)/.$(@F)_$*.dep $< -o $(BUILD_DIR)/$%
	@ $(AR) rc $@ $(BUILD_DIR)/$%
	@ $(RM) $(BUILD_DIR)/$%
	@ $(CXXFILT) < $(BUILD_DIR)/$*.lst > $(BUILD_DIR)/$*.lst.tmp
	@ $(MV) $(BUILD_DIR)/$*.lst.tmp $(BUILD_DIR)/$*.lst
endef

# Rules.
.PHONY : all clean upload monitor upload_monitor

all : $(BUILD_DIR) $(IMAGE).hex $(VERSIONED_HEX_IMAGE).hex flashCube

clean :
	$(RM) $(BUILD_DIR)
	$(RM) *~	# Emacs cruft
	$(RM) *.o *.lst	# Build products
	$(RM) *.hex

$(BUILD_DIR) :
	$(MKDIR) $@

# Used to include:
#   echo '#include <Arduino.h>' > $(BUILD_DIR)/$(SKETCH)_ino.cpp
#   cat $(SKT_INO_SRC) >> $(BUILD_DIR)/$(SKETCH)_ino.cpp
$(SKT_INO_OBJ) : $(SKT_INO_SRC)
	echo "#define SVNVERSION \"$(SVNVERSION)\"" >svnversion.h  # Every compile needs a new svnversion.h
	$(CXX) -c $(subst build/,,$(CXX_FLAGS)) \
	    $(OPT_FLAGS) $(ARD_FLAGS) -I.. \
	    $(patsubst -I..%,-I../..%,$(INC_FLAGS)) \
	    -x c++ $(SKT_INO_SRC) -x none -o build/$(@F)
# The x wackiness tells the compiler that .ino file should be compiled
# as if it were a .cpp file, which is how the makefile used to work:
#	    $(SKETCH)_ino.cpp -o $(@F)

(%.o) : $(ARD_SRC_DIR)/%.c
	$(run-cc)

(%.o) : $(ARD_SRC_DIR)/%.cpp
	$(run-cxx)

(%.o) : %.c
	$(run-cc)

(%.o) : %.cpp
	$(run-cxx)

$(BUILD_DIR)/%.d : %.c
	$(run-cc-d)

$(BUILD_DIR)/%.d : %.cpp
	$(run-cxx-d)

# The multiple "-lm" flags are to work around a linker bug.
$(IMAGE).hex : $(ARD_AR_OBJ) $(LIB_AR_OBJ) $(SKT_AR_OBJ) $(SKT_INO_OBJ)
	$(CC) -lm $(CXX_FLAGS) $(OPT_FLAGS) $(ARD_FLAGS) -L$(BUILD_DIR) \
	    $(SKT_INO_OBJ) $(SKT_LD_FLAG) $(LIB_LD_FLAG) $(ARD_LD_FLAG) \
	    -lm -o $(IMAGE).elf
	$(OBJCOPY) -O ihex -j .eeprom --set-section-flags=.eeprom=alloc,load \
	    --no-change-warnings --change-section-lma .eeprom=0 $(IMAGE).elf \
	    $(IMAGE).eep
	$(OBJCOPY) -O ihex -R .eeprom $(IMAGE).elf $(IMAGE).hex
	$(OBJDUMP) -h -S $(IMAGE).elf | $(CXXFILT) -t > $(IMAGE).lst
	$(SIZE) $(IMAGE).elf

# Produce versioned checkin candidate.
# $(IMAGE) starts with build/, $(VERSIONED_HEX_IMAGE) doesn't.
# copy from build/ to sketch dir and add SVNVERSION
$(VERSIONED_HEX_IMAGE).hex : $(IMAGE).hex
	cp $(IMAGE).hex $(VERSIONED_HEX_IMAGE).hex


# Old upload (faster, if the PORT is right) was:
#- $(AVRDUDE) -V -C$(AVRDUDE_CONF) -p$(MCU) -c$(PROGRAMMER) -P$(PORT) \
	    -b$(UPLOAD_SPEED) -D -Uflash:w:$(IMAGE).hex:i -q
upload : $(IMAGE).hex
	$(kill-monitor)
	./flashCube.py

monitor :
	$(kill-monitor)
	$(run-monitor)

upload_monitor : upload monitor

flashCube : $(VERSIONED_HEX_IMAGE).hex flashCube.py
	cp flashCube.py ../flashCube/
	#svn rm --force ../flashCube/$(wildcard *.hex)  # get rid of old .hex
	cp $(VERSIONED_HEX_IMAGE).hex ../flashCube/    # install new one
	svn add --force ../flashCube/$(VERSIONED_HEX_IMAGE).hex

-include $(wildcard $(BUILD_DIR)/.*.dep))
