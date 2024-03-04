using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkResult<T>
{

}


public interface SucceseNR<T> : INetworkResult<T>
{

}
public interface FailedNR<T> : INetworkResult<T>
{

}