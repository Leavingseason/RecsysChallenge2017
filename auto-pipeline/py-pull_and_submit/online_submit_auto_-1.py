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

def interaction_url(server):
	return server + "/api/online/data/interactions"
	
def offline_submission(server):
	return server + "/api/submission"
def online_submission(server):
	return server + "/api/online/submission"

	
def get_stats():
	req = urllib.request.Request(status_url(SERVER), None, {"Authorization": "Bearer %s" %TOKEN})
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')
	response = json.loads(content)  

	return parse(response['current']['updated_at'])

def is_ready(): 
	status_date = get_stats().date() 
	print('get_stats().date() = ' + str(status_date))
	print('datetime.date.today() = ' + str(datetime.date.today()))
	
	return status_date == datetime.date.today()

def download_items():
	req = urllib.request.Request(items_url(SERVER), None, {"Authorization": "Bearer %s" %TOKEN})
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')		
	
	fp = open(PULL_DATA_PATH+'\\target_items_'+datetime.date.today().isoformat()+'.txt', "w")
	fp.write(content)
	fp.close()
	#return parser.select(TMP_ITEMS, lambda x: True, parser.build_item, lambda x: int(x[0]))

def download_acceptsubmission():
	req = urllib.request.Request(online_submission(SERVER), None, {"Authorization": "Bearer %s" %TOKEN})
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')		
	
	fp = open(PULL_DATA_PATH+'\\accepted_pairs\\accepted_pairs_'+datetime.date.today().isoformat()+'.txt', "w")
	fp.write(content)
	fp.close()
	
def download_interactions():
	req = urllib.request.Request(interaction_url(SERVER), None, {"Authorization": "Bearer %s" %TOKEN})
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')		
	
	fp = open(PULL_DATA_PATH+'\\interactions\\interaction_'+datetime.date.today().isoformat()+'.txt', "w")
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
	
	fp = open(PULL_DATA_PATH+'\\target_users_'+datetime.date.today().isoformat()+'.txt', "w")
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
	download_target_users()
	download_items()

def offline_submit(filename):
	rd = open(filename,'r')
	content=rd.read()
	rd.close() 
	content = content.encode('utf-8')
	req = urllib.request.Request(url=offline_submission(SERVER), data=content, headers={"Authorization": "Bearer %s" %TOKEN}, method='POST')
	response=urllib.request.urlopen(req)
	content=response.read().decode('utf-8')	
	print(content)
	
def online_submit(filename):
	rd = open(filename,'r')
	content=rd.read()
	rd.close() 
	content = content.encode('utf-8')
	req = urllib.request.Request(url=online_submission(SERVER), data=content, headers={"Authorization": "Bearer %s" %TOKEN}, method='POST')
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

def submit_file_online(file):
	while True:
		try:
			print("submitting "+file)
			online_submit(file)
			print("submitting successfully")
			break 
		except KeyboardInterrupt:
			break
		except:
			print("exception :"+str(sys.exc_info()[0])+"\n")
			
		
if __name__ == "__main__":
	
	submit_file_online(r'\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v-1.csv')	