using System;
using UnityEngine;

namespace ManualBase64
{
    public class ManualBase64Encoder
    {
        byte[] main_data;
        int length,length2;
        int cblock;
        int padC;

        public ManualBase64Encoder(byte[] rawBytes)
        {
            main_data = rawBytes;
            length = rawBytes.Length;
            
            if((length%3)==0)
            {
                padC=0;
                cblock=length/3;
            }
            else
            {
                padC=3-(length%3);
                cblock=(length+padC)/3;
            }
            
            length2=length+padC;
        }

        public char[] Encode()
        {
            byte[] main_data_2;
            main_data_2=new byte[length2];

            for(int x=0;x<length2;x++)
            {
                if(x<length)
                {
                    main_data_2[x]=main_data[x];
                }
                else
                {
                    main_data_2[x]=0;
                }
            }
      
            byte b1;
            byte b2;
            byte b3;
            byte temp;
            byte temp1;
            byte temp2;
            byte temp3;
            byte temp4;
            byte[] buffer=new byte[cblock*4];
            char[] result=new char[cblock*4];

            for (int x=0;x<cblock;x++)
            {
                b1=main_data_2[x*3];
                b2=main_data_2[x*3+1];
                b3=main_data_2[x*3+2];

                temp1=(byte)((b1 & 252)>>2);

                temp=(byte)((b1 & 3)<<4);
                temp2=(byte)((b2 & 240)>>4);
                temp2+=temp;

                temp=(byte)((b2 & 15)<<2);
                temp3=(byte)((b3 & 192)>>6);
                temp3+=temp;

                temp4=(byte)(b3 & 63);

                buffer[x*4]=temp1;
                buffer[x*4+1]=temp2;
                buffer[x*4+2]=temp3;
                buffer[x*4+3]=temp4;
            }

            for(int x=0;x<cblock*4;x++)
            {
                result[x]=look(buffer[x]);
            }

            switch(padC)
            {
                case 0:
                    break;
                case 1:
                    result[cblock*4-1]='=';
                    break;
                case 2:
                    result[cblock*4-1]='=';
                    result[cblock*4-2]='=';
                    break;
                default:
                    break;
            }

            return result;
        }

        private char look(byte b)
        {
            char[] table=new char[64] {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9','+','/'};

            if((b>=0)&&(b<=63))
            {
                return table[(int)b];
            }

            return ' ';
        }
    }
}