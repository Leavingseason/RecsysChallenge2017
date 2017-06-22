'''
Online example

Uses the offline mode to make predictions 
for the online challenge.

by Daniel Kohlsdorf
'''
import urllib.request
import time 
import sys
import json
from dateutil.parser import parse
import datetime
import parser
#from recommendation_worker import *

TMP_ITEMS	= "data/current_items.csv"
TMP_SOLUTION = "data/current_solution.csv"

MODEL		= "data/recsys2017.model" # Model from offline training
USERS_FILE   = "data/users.csv"		# Online user data

PULL_DATA_PATH = r'\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data'
PULL_DATA_LOG_FILE = PULL_DATA_PATH + '\\pull-dates.txt'

cur_date_flag = ''

TOKEN  = "bGVhdmluZ3NlYXNvbjdiODFkYTRlLTM4MGUtNGZkOC1iYTVjLTM5MjA0M2VhOTQ5Yw==" # your key 
SERVER = "https://recsys.xing.com"

def header(token):
	return {"Authorization" : "Bearer %s" %TOKEN}

def post_url(server):
	return server + "/api/online/submission"

def status_url(server):
	return server + "/api/online/data/status"

def users_url(server):
	return server + "/api/online/data/users"

def items_url(server):
	return server + "/api/online/data/items"
	
def offline_submission(server):
	return server + "/api/submission"

def get_stats():
	req = urllib.request.Request(status_url(SERVER), None, {"Authorization": "Bearer %s" %TOKEN})
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')
	response = json.loads(content)  

	return parse(response['current']['updated_at'])

def is_ready(): 
	global cur_date_flag
	existing_dates = set()
	with open(PULL_DATA_LOG_FILE,'r') as rd:
		for date_str in rd.readlines():
			existing_dates.add(date_str.rstrip())
	status_date = get_stats().date() 
	cur_date_flag = str(status_date)
	print('get_stats().date() = ' + cur_date_flag)
	print('datetime.date.today() = ' + str(datetime.date.today()))
	
	return cur_date_flag not in existing_dates

def download_items():
	req = urllib.request.Request(items_url(SERVER), None, {"Authorization": "Bearer %s" %TOKEN})
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')		
	global cur_date_flag
	fp = open(PULL_DATA_PATH+'\\target_items_'+cur_date_flag+'.txt', "w")
	fp.write(content)
	fp.close()
	#return parser.select(TMP_ITEMS, lambda x: True, parser.build_item, lambda x: int(x[0]))
	

def user_info(user_ids):
	return parser.select(
		USERS_FILE, 
		lambda x: int(x[0]) in user_ids and "NULL" not in x,
		parser.build_user, 
		lambda x: int(x[0])
	)

def download_target_users():
	req = urllib.request.Request(users_url(SERVER), None, {"Authorization": "Bearer %s" %TOKEN})
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')	
	global cur_date_flag
	fp = open(PULL_DATA_PATH+'\\target_users_'+cur_date_flag+'.txt', "w")
	fp.write(content)
	fp.close()
	
	'''
	user_ids = set([int(uid) for uid in content.split("\n") if len(uid) > 0])
	
	with open(PULL_DATA_PATH+'\\target_users_'+datetime.date.today().isoformat()+'.txt','w') as wt:
		for uid in user_ids:
			wt.write(str(uid)+"\n")
	'''	
	#return user_ids
				  
def process():
	print('downloading data...')
	download_target_users()
	download_items()
	global cur_date_flag
	with open(PULL_DATA_LOG_FILE,'a') as wt:
		wt.write(cur_date_flag+'\n')

def offline_submit(filename):
	rd = open(filename,'r')
	content=rd.read()
	rd.close() 
	content = content.encode('utf-8')
	req = urllib.request.Request(url=offline_submission(SERVER), data=content, headers={"Authorization": "Bearer %s" %TOKEN}, method='POST')
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')	
	print(content)
	
def submit():
	http = httplib2.Http()	
	filename = TMP_SOLUTION
	with open(filename, 'r') as content_file:
		content = content_file.read()
		response = http.request(post_url(SERVER), method="POST", body=content,
			headers=header(TOKEN)
		)[1].decode("utf-8")
		print("SUBMIT: " + filename + " " + response)

def usage_test():
	'''
	req = urllib.request.Request(status_url(SERVER), None, {"Authorization": "Bearer %s" %TOKEN})
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')
	print(content)
	response = json.loads(content)  
	print(response)
	'''
	print(is_ready())
	
	process()
	
		
if __name__ == "__main__":

	#usage_test()
	#offline_submit(r'\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\recsys17-pred-submit.csv')
	
	
	last_submit = None
	while True:
		try:
			if is_ready() and last_submit != datetime.date.today():
				print('data ready.')
				process()
				last_submit = datetime.date.today()
				#submit()
			else:
				print("Not ready yet: " + str(datetime.date.today()))
			time.sleep(600)
		except KeyboardInterrupt:
			break
		except:
			print("exception :"+str(sys.exc_info()[0])+"\n")
	
	
	