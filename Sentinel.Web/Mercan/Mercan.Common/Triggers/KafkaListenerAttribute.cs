using System;

public class KafkaListenerAttribute : Attribute
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
    public string BootstrapServers { get; set; }

    public KafkaListenerAttribute(string topic)
    {
        Topic = topic;
    }


    public KafkaListenerAttribute(string bootstrapServers, string groupId string topic)
    {
        this.Topic = topic;
        this.BootstrapServers = bootstrapServers;
        this.GroupId = groupId;
    }
}