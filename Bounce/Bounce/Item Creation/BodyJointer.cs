using System;
using System.Collections;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bounce
{
    public static class ItemJointer
    {
        public static FixedRevoluteJoint CenterRevolute(PhysicalScene scene, PhysicalItem item)
        {
            return JointFactory.CreateFixedRevoluteJoint(scene.World, item.Body, Vector2.Zero, item.Body.Position);
        }

        public static RevoluteJoint CenterRevolute(PhysicalScene scene, PhysicalItem item1, PhysicalItem item2)
        {
            return JointFactory.CreateRevoluteJoint(scene.World, item1.Body, item2.Body, Vector2.Zero) ;
        }
    }
}