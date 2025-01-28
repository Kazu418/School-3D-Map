using UnityEngine;
using System.Linq;

public class Function : MonoBehaviour
{
    public displayList displayList;
    
    void OnEnable(){

    }

    public LocationsList FindOptionalLocation(int FloorNum = -1,int HubNum = -1){
        LocationsList[] locations = displayList.locations;
        LocationsList[] filteredLocations;
    
        if(FloorNum != -1 && HubNum != -1){
            filteredLocations = locations.Where(location => (location.FloaNumber == FloorNum)&&(location.HubNumber == HubNum)).ToArray();
        }
        else if(FloorNum != -1){
            filteredLocations = locations.Where(location => (location.FloaNumber == FloorNum)).ToArray();
        }
        else{
            filteredLocations = locations.Where(location => (location.HubNumber == HubNum)).ToArray();
        }
        return filteredLocations[0];
        
    }
}
