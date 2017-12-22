using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports; 
using System.Threading; 
using System.Windows.Forms; // temp use during debug 

namespace Sinconizacion_EXactus
{
    class HyperTerminalAdapter
    {
        SerialPort oSerialPort = new SerialPort(); 
	 
    // Allow the user to set the appropriate properties. 
	    public int BaudRate = 19200; 
	    public int DataBits = 7; 
	    public int ReadTimeout = 20000; 
	    public int WriteTimeout = 5000; 
	    public string PortName = "COM1"; 
	    public string Handshake = ""; 
	    public string Name = "user"; 
	    public string DataReceived = "";
        public string sParity = "even"; 
	    public int iStopBits = 1; 
	 
	    public HyperTerminalAdapter() { 
	        this.Configure(); 
	    } 
	 
	    public void Configure() { 
	        oSerialPort.PortName = this.PortName; 
	        oSerialPort.BaudRate = this.BaudRate; 
	        oSerialPort.DataBits = this.DataBits; 
	        oSerialPort.ReadTimeout = this.ReadTimeout; 
	        oSerialPort.WriteTimeout = this.WriteTimeout;
            oSerialPort.RtsEnable = true;
            oSerialPort.DtrEnable = true;
            oSerialPort.DiscardNull = true;
	 
	        oSerialPort.Handshake = System.IO.Ports.Handshake.None; 
	         
	        if(this.sParity == "even"){ 
	            oSerialPort.Parity = Parity.Even; 
	        }else if(this.sParity == "odd"){ 
	            oSerialPort.Parity = Parity.Odd; 
	        }else if(this.sParity == "mark"){ 
	            oSerialPort.Parity = Parity.Mark; 
	        }else if(this.sParity == "space"){ 
	            oSerialPort.Parity = Parity.Space; 
	        } else { 
	            oSerialPort.Parity = Parity.None; 
	        } 
	 
	        if(this.iStopBits == 0){ 
	            oSerialPort.StopBits = StopBits.None; 
	        }else if(this.iStopBits == 1.5){ 
	            oSerialPort.StopBits = StopBits.OnePointFive; 
	        }else if(this.iStopBits == 2){ 
	            oSerialPort.StopBits = StopBits.Two; 
	        } else { 
	            oSerialPort.StopBits = StopBits.One; 
	        } 
	 
	        MessageBox.Show("Configured"); 
	    } 
	 
	    public void Connect() { 
	        try { 
	            if (!oSerialPort.IsOpen) { 
	                oSerialPort.Open(); 
	                MessageBox.Show("Connected"); 
	            } // else already open 
	        } catch (Exception e1) {  
	                MessageBox.Show("Error: Connection is in use or is not available: \n\n" + e1);  
	        } 
	    } 
	 
	    public void Disconnect() { 
	        try{ 
	            if (oSerialPort.IsOpen) { 
	                oSerialPort.Close(); 
	                MessageBox.Show("Disconnected"); 
	            } //else not open 
	        } catch { } 
	    } 
	 
	    public void Write(string sData /* string data to write to the port */ ) { 
	        if (oSerialPort.IsOpen) { 
	            try { 
	                oSerialPort.WriteLine(sData); 
	                MessageBox.Show("Command Issued: " + sData); 
	            } catch { } 
	        } 
	    } 
	 
	    public string Read() { 
	        try { 
	            this.DataReceived = oSerialPort.ReadLine().ToString(); 
	            MessageBox.Show("Leyendo "+this.DataReceived); 
	            return (this.DataReceived);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Leyendo" + ex.Message); 
	            return ""; 
	        } 
	    } 
    }
}
