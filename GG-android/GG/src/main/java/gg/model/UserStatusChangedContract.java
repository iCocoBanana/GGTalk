package gg.model;

import com.oraycn.es.communicate.common.Consts;

import org.jboss.netty.buffer.ChannelBuffer;
import org.jboss.netty.buffer.DynamicChannelBuffer;

import java.io.IOException;
import java.nio.ByteOrder;

import gg.example.utils.SerializeHelper;
import io.netty.buffer.ByteBuf;

/**
 * Created by ZN on 2015/9/5.
 */
public class UserStatusChangedContract {

    private String UserID;
    private int NewStatus;

    public UserStatusChangedContract() {
    }

    public UserStatusChangedContract(String userID, int newStatus) {
        this.UserID = userID;
        this.NewStatus = newStatus;
    }

    public String getUserID() {
        return UserID;
    }

    public void setUserID(String userID) {
        UserID = userID;
    }

    public UserStatus getStatus() {
        return UserStatus.getUserStatusByCode(this.NewStatus);
    }

    public void setNewStatus(int newStatus) {
        NewStatus = newStatus;
    }

    public void deserialize(byte[] info) throws IOException {
        ByteBuf buffer = SerializeHelper.wrappedBuffer(info);
        buffer.readInt();
        this.NewStatus = buffer.readInt();
        this.UserID = SerializeHelper.readStrIntLen(buffer);

    }

    public byte[] serialize() throws IOException {
        {
            ChannelBuffer body = new DynamicChannelBuffer(ByteOrder.LITTLE_ENDIAN, Consts.DYNAMIC_BUFFER_LEN);
            byte[] userBytes = this.getUserID().getBytes("UTF8");
            body.writeByte(8 + userBytes.length);
            body.writeInt(this.NewStatus);
            body.writeInt(userBytes.length);
            body.writeBytes(userBytes);

            return body.array();
        }
    }
}
