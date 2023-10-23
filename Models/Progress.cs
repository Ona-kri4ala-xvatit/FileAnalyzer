using FileAnalyzer.Base;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.Models
{
    public class Progress : ViewModelBase
    {
        int _Min = 0;
        int _Max = 1000;
        int _Pos = 0;

        public int Min
        {
            get => _Min;
            set
            {
                base.PropertyChangeMethod(out _Min, value);
            }
        }

        public int Max
        {
            get { return _Max; }
            set
            {
                base.PropertyChangeMethod(out _Max, value);
            }
        }

        public int Pos
        {
            get => _Pos;
            set
            {
                base.PropertyChangeMethod(out _Pos, value);
            }
        }

        public Progress()
        {
        }

        internal void StartProcessing()
        {
            Min = 0;
            Max = 10;
            Pos = 0;
            Task.Factory.StartNew(() =>
            {
                for (int i = Min; i < Max; i++)
                {
                    Pos = i;
                    Thread.Sleep(1000);
                }
                Pos = Max;
            });
        }
    }
}
