using ExtraDrug.Core.Models;

namespace ExtraDrug.Helpers;

public class StateManager
{
    public Dictionary<RequestState, ICollection<RequestState>> Trans { get; } = 
        new Dictionary<RequestState, ICollection<RequestState>>() {
            { RequestState.Pending , new List<RequestState>() { RequestState.Canceled  ,RequestState.Accepted }},
            { RequestState.Accepted , new List<RequestState>() { RequestState.Canceled  ,RequestState.Recieved }},
            { RequestState.Canceled , new List<RequestState>() { }},
            { RequestState.Recieved , new List<RequestState>() { }},
        };
    public bool validStateChange(RequestState oldState , RequestState newState)
    {
        return Trans[oldState].Contains(newState);
    }
}
