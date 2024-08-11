using System.Collections.Generic;
using System.Linq;

public class InputInfos
{
    private readonly List<Inputs> _realInputs = new List<Inputs>();
    private readonly List<Inputs> _predictedInputs = new List<Inputs>();

    public IReadOnlyList<Inputs> RealInputs => _realInputs;
    public IReadOnlyList<Inputs> PredictedInputs => _predictedInputs;

    public void AddRealInput(
        Inputs input
        )
    {
        _realInputs.Add( input );
    }

    public void AddPredictedInput(
        Inputs input
        )
    {
        Inputs team_input = _predictedInputs.FirstOrDefault( selected_input => selected_input.Team == input.Team );

        if( !team_input.Equals( Inputs.None() ) )
        {
            _predictedInputs.Remove( team_input );
        }

        _predictedInputs.Add( input );
    }

    public bool HasInputsFromAllPlayers()
    {
        return _realInputs.Count == ScenarioManager.Instance.ActiveScenario.ConnexionHandler.PlayerCount;
    }

    public bool RealAndPredictedInputsMatch()
    {
        if( _realInputs.Count != _predictedInputs.Count )
        {
            return false;
        }

        foreach( Inputs input in _realInputs )
        {
            if( _predictedInputs.FirstOrDefault( selected_input => selected_input.Team == input.Team ).Equals( input ) )
            {
                return false;
            }
        }

        return true;
    }
}
