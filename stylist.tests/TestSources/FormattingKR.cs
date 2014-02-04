public class AbandonedMutexException : SystemException { 

    private int m_MutexIndex = -1; 
    private Mutex m_Mutex = null; 

    public AbandonedMutexException() 
        : base(Environment.GetResourceString("Threading.AbandonedMutexException")) {
        SetErrorCode(__HResults.COR_E_ABANDONEDMUTEX);
    }
}

class Class
{
	void Print() {
		var used = false;
	}
	void Write() { }

	void WriteOk() {
		for (int i = 0; i < 10; i++) {
			Console.WriteLine(i);
		}
	}

	void WriteOk() {
	for (int i = 0; i < 10; i++) {
		Console.WriteLine(i);
	}
	}
}

//Missing indentation (Line: 26)