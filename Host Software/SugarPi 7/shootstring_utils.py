import re
# loop = '{0,100,10 R* T }'
# shootstring =  "][close comments]*NEWLINE_TOKEN*[2 layer 60 and 30 degrees]*NEWLINE_TOKEN**NEWLINE_TOKEN*[Usual] S V0 I1 LO LG3*NEWLINE_TOKEN**NEWLINE_TOKEN*[Spatial Calibration Images]*NEWLINE_TOKEN*R0 E0 E80 R20 R0*NEWLINE_TOKEN*{0,351,90*NEWLINE_TOKEN*R* T*NEWLINE_TOKEN*}*NEWLINE_TOKEN**NEWLINE_TOKEN*[Set Elevation to lower level] *NEWLINE_TOKEN*R0 E0 E60 R20 R0*NEWLINE_TOKEN*{0,351,20*NEWLINE_TOKEN*R* T*NEWLINE_TOKEN*}*NEWLINE_TOKEN**NEWLINE_TOKEN*[Set Elevation to mid level]  *NEWLINE_TOKEN*R0 E0 E30 R20 R0*NEWLINE_TOKEN*{0,351,20*NEWLINE_TOKEN*R* T*NEWLINE_TOKEN*}*NEWLINE_TOKEN**NEWLINE_TOKEN*[Usual] E0 R0 S LO LB6*NEWLINE_TOKEN*[done]"

def expand_loop(loop):
	# Expands {stuff } into stuff stuff stuff
	# Expects loop to be split by , or ' '
	# This means {1 10 3 stuff} is equivalent to {1,10,3 stuff}
	# Not sure if that is a problem- we might want to sanitize shoot strings somewhere
	loop = loop.lstrip('{').rstrip('}')
	tolkens = re.split(',|\s', loop)
	#print(tolkens)
	start = int(tolkens[0])
	end = int(tolkens[1])
	step = int(tolkens[2])
	ans = ''

	i = start
	while(i <= end):
		for tolken in tolkens[3:]:
			ans += re.sub('\*', str(i), tolken) + ' '
		i += step
	return ans

# expand_loop(loop)

def apply_expand_loop(match):
	return expand_loop(match.group(0))

def string_to_token_list(shootstring):
	shootstring = shootstring.replace('*NEWLINE_TOKEN*',' ') # Get rid of this
	shootstring = shootstring.replace('\n',' ') # Get rid of new lines
	sstring = re.sub('\[.+?\]','',shootstring)  # Deleting [ comments ] from the string
	sstring = re.sub('\]','',sstring) # Delete floater ] to preserve backwards compatability
	# print(sstring)
	sstring = re.sub('({.+?})', apply_expand_loop, sstring) # Expand loops
	sstring = re.sub('\s+', ' ', sstring).strip() # Remove excess whitespace
	# print(sstring)
	return sstring.split(' ')

# tokens = string_to_token_list
# st = clean_shootstring(shootstring)

