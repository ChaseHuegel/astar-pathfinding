using System;
using System.Threading;
using Swordfish;

namespace Swordfish.Threading
{

public class ThreadWorker
{
	private volatile bool stop = false;
	private volatile bool pause = false;

	private Thread thread = null;
	private Action handle;

	public ThreadWorker(Action handle, bool runOnce = false)
	{
		this.handle = handle;

		if (runOnce)
			this.thread = new Thread(Handle);
		else
			this.thread = new Thread(Tick);
	}

	public void Start()
	{
		stop = false;
		pause = false;
		thread.Start();
	}

	public void Stop()
	{
		stop = true;
	}

	public void Restart()
	{
		stop = false;
		pause = false;
		thread.Abort();
		thread.Start();
	}

	public void Pause()
	{
		pause = true;
	}

	public void Unpause()
	{
		pause = false;
	}

	public void TogglePause()
	{
		pause = !pause;
	}

	public void Kill()
	{
		thread.Abort();
	}

	private void Handle()
	{
		handle();
		Kill();
	}

	private void Tick()
	{
		while (stop == false)
		{
			while (pause == false)
			{
				//	If handle is no longer valid, kill the thread
				if (handle == null) Kill();

				handle();
			}

			Thread.Sleep(200);	//	Sleep when paused
		}

		//	Stopped thread safely
	}
}

}