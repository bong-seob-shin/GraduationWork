using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    public class RecvBuffer
    {
        // [r][][][w][][][][][][]
        ArraySegment<byte> _buffer;
        int _readPos;
        int _writePos;

        public RecvBuffer(int bufferSize) 
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public int DataSize { get { return _writePos - _readPos; } }
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        public ArraySegment<byte> ReadSegment   
        {
            // 데이터가 있는 데이터 범위
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
        }

        public ArraySegment<byte> WriteSegment    
        {
            // 사용할 수 있는 유효 범위
            get { return new ArraySegment<byte>(_buffer.Array,_buffer.Offset+_writePos, FreeSize); }
        }

        public void Clean()
        {
            // 중간중간에 정리 --> 정리 안하면 r와 w가 버퍼 끝까지 밀리게 될 수도 있기 때문
            int dataSize = DataSize;
            if (dataSize == 0) // Read와 Write가 정확히 겹치는 상황(= 클라에서 보내준 데이터를 모두 처리한 상태)
            {
                // 남은 데이터가 없으면 복사하지 않고 커서 위차만 리셋
                _readPos = _writePos = 0;
            }
            {
                // 남은 찌끄레기가 있으면 시작 위치로 복사
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        public bool OnRead(int numOfBytes) 
        {
            if (numOfBytes > DataSize)
                return false;

            _readPos += numOfBytes;
            return true;
        }

        public bool OnWrite(int numOfBytes)
        {
            if (numOfBytes > FreeSize)
                return false;

            _writePos += numOfBytes;
            return true;
        }

    }
}
