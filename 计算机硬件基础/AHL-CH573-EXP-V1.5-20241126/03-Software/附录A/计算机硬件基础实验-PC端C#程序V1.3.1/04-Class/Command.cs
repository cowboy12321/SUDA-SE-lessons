using Newtonsoft.Json;
using System.Collections.Generic;

namespace GEC_LAB._04_Class
{
    public class Command
    {
        private string LOG = "Command";
        private delegate List<byte> MakeCMD(OrderCommand cmd);
        private delegate void AdditonEach(OrderCommand cmd);
        private List<OrderCommand> cmds = new();
        private readonly double MinVolta = 0;
        private readonly double MaxVolta=3.3;
        private bool inFlag = false;
        private bool outFlag=false;
        public delegate void ChangedFunc(short no, double val);
        public static ChangedFunc? DigitalControled;
        private readonly Dictionary<OrderCommand.Type, MakeCMD> makeCmds = new()
        {
            { OrderCommand.Type.AnalogIn , cmd => makeOneCommand(0, cmd)},
            { OrderCommand.Type.AnalogOut , cmd => makeOneCommand(1, cmd)},
            { OrderCommand.Type.DigitalIn , cmd => makeOneCommand(2, cmd)},
            { OrderCommand.Type.DigitalOut , cmd => makeOneCommand(3, cmd)},
        };
        public void sendWithoutLog()
        {
            Gobals.logger?.info(LOG, "发送命令：" + JsonConvert.SerializeObject(cmds));

            List<byte> temp = new();
            if (inFlag)
            {
                temp.Add(0xa0);
            }
            if (outFlag)
            {
                temp.Add(0xa1);
            }
            foreach (var cmd in cmds)
            {

                //一次最多发送16字节的数据
                List<byte> t = makeCmds[cmd.type](cmd);
                if (temp.Count + t.Count > 16)
                {
                    EmuartHandler.Instance.sendMessage(temp.ToArray());
                    temp.Clear();
                }
                temp.AddRange(t);
            }
            if (temp.Count > 0) EmuartHandler.Instance.sendMessage(temp.ToArray());
        }
        public void send()
        {
            Gobals.logger?.info(LOG, "发送命令：" + JsonConvert.SerializeObject(cmds));
            sendWithoutLog();
        }
        public Command openAnalog(short pin)
        {
            inFlag = true;
            cmds.Add(new OrderCommand { type = OrderCommand.Type.AnalogIn, pin = pin});
            return this;
        }
        public Command analogControl(short pin, double value)
        {
            outFlag = true;
            value = value < MinVolta ? MinVolta : (value > MaxVolta ? MaxVolta : value);
            cmds.Add(new OrderCommand { type = OrderCommand.Type.AnalogOut, pin = pin, value = (int)((value - MinVolta) / MaxVolta * 4095 + 0.5) });
            return this;
        }
        public Command openGPIO(short pin)
        {
            inFlag = true;
            cmds.Add(new OrderCommand { type = OrderCommand.Type.DigitalIn, pin = pin });
            return this;
        }
        public Command digitalControl(short pin, int value)
        {
            outFlag = true;
            cmds.Add(new OrderCommand { type = OrderCommand.Type.DigitalOut, pin = pin, value = value });
            return this;
        }
        private static List<byte> makeOneCommand(int code, OrderCommand cmd)
        {
            List<byte> res = new List<byte>() {
                (byte)((code & 0x7) << 5 | (cmd.pin >> 6) & 0x3),
                (byte)(cmd.pin & 0x7f)
            };
            if (code == 1)
            {
                res[0] |= (byte)(cmd.value & 0x1f);
                res.Add((byte)((cmd.value >> 5) & 0x7f));
            }
            if (code == 3)
            {
                if (cmd.value != 0)
                {
                    res[0] |= 0x1;
                }
                DigitalControled?.Invoke(cmd.pin,cmd.value);
            }
            return res;
        }
        public class OrderCommand
        {
            public Type type;
            public short pin { get; set; }
            public int value { get; set; }
            public enum Type
            {
                AnalogIn, AnalogOut, DigitalIn, DigitalOut,
            }
        }
    }
}
