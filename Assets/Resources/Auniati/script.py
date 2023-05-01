import json
import os

# read the locData.json file

# CHANGE THE PATH OF THE OLD JSON FILE HERE. USE ABSOLUTE PATH IF OUTSIDE THE FOLDER.
# IF INSIDE THE FOLDER, USE RELATIVE PATH.

old_path = "locData.json"
new_path = "locData2.json"

with open(old_path) as f:
    data = json.load(f)

# get the data from the json file
# print(data)
locations = data["Locs"]

new_arr = []

# add a name field to the locs
for loc in locations:
    loc["name"] = "img" + "_" + str(loc["x"]) + "_" + str(loc["y"])

for loc in locations:
    new_dict = {
        "name": "",
        "data": {
            "left": "",
            "right": "",
            "up": "",
            "down": "",
            "transition": "",
            "transVid": "",
            "siteName": "",
            "initImage": "",
            "tabVideo": "",
            "tabText": "",
            "navTextLeft": "",
            "navTextRight": "",
            "navTextUp": "",
            "navTextDown": "",
            "ambientAudio": "",
            "rotation": 0,
        },
    }
    # find the loc with the next x value and same y value
    min_dist = 1000
    for loc2 in locations:
        if loc["y"] == loc2["y"]:
            if loc2["x"] - loc["x"] < min_dist and loc2["x"] - loc["x"] > 0:
                min_dist = loc2["x"] - loc["x"]
                new_dict["data"]["down"] = loc2["name"]
    # find the loc with the previous x value and same y value
    min_dist = 1000
    for loc2 in locations:
        if loc["y"] == loc2["y"]:
            if loc["x"] - loc2["x"] < min_dist and loc["x"] - loc2["x"] > 0:
                min_dist = loc["x"] - loc2["x"]
                new_dict["data"]["up"] = loc2["name"]
    # find the loc with the next y value and same x value
    min_dist = 1000
    for loc2 in locations:
        if loc["x"] == loc2["x"]:
            if loc2["y"] - loc["y"] < min_dist and loc2["y"] - loc["y"] > 0:
                min_dist = loc2["y"] - loc["y"]
                new_dict["data"]["right"] = loc2["name"]
    # find the loc with the previous y value and same x value
    min_dist = 1000
    for loc2 in locations:
        if loc["x"] == loc2["x"]:
            if loc["y"] - loc2["y"] < min_dist and loc["y"] - loc2["y"] > 0:
                min_dist = loc["y"] - loc2["y"]
                new_dict["data"]["left"] = loc2["name"]
    # new_dict['name'] = 'img' + str(loc['x']) + '_' + str(loc['y'])
    new_dict["name"] = loc["name"]
    try:
        new_dict["data"]["tabText"] = loc["data"]["tabText"]
    except:
        pass
    try:
        new_dict["data"]["navTextLeft"] = loc["data"]["navTextLeft"]
    except:
        pass
    try:
            new_dict["data"]["navTextRight"] = loc["data"]["navTextRight"]
    except:
        pass
    try:
            new_dict["data"]["navTextUp"] = loc["data"]["navTextUp"]
    except:
        pass
    try:
            new_dict["data"]["navTextDown"] = loc["data"]["navTextDown"]
    except:
        pass
    try:
            new_dict["data"]["ambientAudio"] = loc["data"]["ambientAudio"]
    except:
        pass
    try:    
            new_dict["data"]["rotation"] = loc["data"]["rotation"]
    except:
        pass
    try:
            new_dict["data"]["tabVideo"] = loc["data"]["tabVideo"]
    except:
        pass
    try:
        if loc["data"]["transition"] == "":
            new_dict["data"]["transition"] = False
        else:
            new_dict["data"]["transition"] = True
            new_dict["data"]["transVid"] = loc["data"]["transition"]["transitionVideo"]
            new_dict["data"]["siteName"] = loc["data"]["transition"]["siteName"]
            new_dict["data"]["initImage"] = "img_" + str(loc["data"]["transition"]["initialLocation"]["x"]) + "_" + str(loc["data"]["transition"]["initialLocation"]["y"])
    except:
        pass
    

    # don't add the fields if they are empty
    for key in list(new_dict["data"].keys()):
        if new_dict["data"][key] == "":
            del new_dict["data"][key]
    new_arr.append(new_dict)

final_dict = {"Locs": new_arr}

# save the new data to a new json file
with open(new_path, "w") as f:
    json.dump(final_dict, f, indent=4)

print("Completed successfully!")
