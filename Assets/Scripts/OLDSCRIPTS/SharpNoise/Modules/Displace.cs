﻿using System;

namespace SharpNoise.Modules
{
    /// <summary>
    /// Noise module that uses three source modules to displace each
    /// coordinate of the input value before returning the output value from
    /// a source module.
    /// </summary>
    /// <remarks>
    /// Unlike most other noise modules, the index value assigned to a source
    /// module determines its role in the displacement operation:
    /// 
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Source module 0 (left in the diagram) outputs a value.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Source module 1 specifies the offset to
    /// apply to the x coordinate of the input value.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Source module 2 specifies the
    /// offset to apply to the y coordinate of the input value.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Source module 3 specifies the offset
    /// to apply to the z coordinate of the input value.
    /// </description>
    /// </item>
    /// </list>
    /// 
    /// The <see cref="GetValue"/> method modifies the ( x, y, z ) coordinates of
    /// the input value using the output values from the three displacement
    /// modules before retrieving the output value from the source module.
    ///
    /// The Turbulence noise module is a special case of the
    /// displacement module; internally, there are three Perlin-noise modules
    /// that perform the displacement operation.
    ///
    /// This noise module requires four source modules.
    /// </remarks>
    [Serializable]
    public class Displace : Module
    {
        /// <summary>
        /// Gets or sets the first source module
        /// </summary>
        public Module Source0
        {
            get { return this.SourceModules[0]; }
            set { this.SourceModules[0] = value; }
        }

        /// <summary>
        /// Gets or sets the x displacement module.
        /// </summary>
        /// <remarks>
        /// The <see cref="GetValue"/> method displaces the input value by adding the output
        /// value from this displacement module to the x coordinate of the
        /// input value before returning the output value from the source
        /// module.
        /// </remarks>
        public Module XDisplace
        {
            get { return this.SourceModules[1]; }
            set { this.SourceModules[1] = value; }
        }

        /// <summary>
        /// Gets or sets the y displacement module.
        /// </summary>
        /// <remarks>
        /// The <see cref="GetValue"/> method displaces the input value by adding the output
        /// value from this displacement module to the x coordinate of the
        /// input value before returning the output value from the source
        /// module.
        /// </remarks>
        public Module YDisplace
        {
            get { return this.SourceModules[2]; }
            set { this.SourceModules[2] = value; }
        }

        /// <summary>
        /// Gets or sets the z displacement module.
        /// </summary>
        /// <remarks>
        /// The <see cref="GetValue"/> method displaces the input value by adding the output
        /// value from this displacement module to the x coordinate of the
        /// input value before returning the output value from the source
        /// module.
        /// </remarks>
        public Module ZDisplace
        {
            get { return this.SourceModules[3]; }
            set { this.SourceModules[3] = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Displace()
            : base(4)
        {
        }

        /// <summary>
        /// Sets all displacement modules in one go.
        /// </summary>
        /// <param name="xDisplaceModule">
        /// Displacement module that displaces the x
        /// coordinate of the input value.
        /// </param>
        /// <param name="yDisplaceModule">
        /// Displacement module that displaces the y
        /// coordinate of the input value.
        /// </param>
        /// <param name="zDisplaceModule">
        /// Displacement module that displaces the z
        /// coordinate of the input value.
        /// </param>
        /// <remarks>
        /// The <see cref="GetValue"/> method displaces the input value by adding the output
        /// value from each of the displacement modules to the corresponding
        /// coordinates of the input value before returning the output value
        /// from the source module.
        ///
        /// This method assigns an index value of 1 to the x displacement
        /// module, an index value of 2 to the y displacement module, and an
        /// index value of 3 to the z displacement module.
        /// </remarks>
        public void SetDisplacementModules(Module xDisplaceModule, Module yDisplaceModule, Module zDisplaceModule)
        {
            this.XDisplace = xDisplaceModule;
            this.YDisplace = yDisplaceModule;
            this.ZDisplace = zDisplaceModule;
        }

        /// <summary>
        /// See the documentation on the base class.
        /// <seealso cref="Module"/>
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <returns>Returns the computed value</returns>
        public override double GetValue(double x, double y, double z)
        {
            // Get the output values from the three displacement modules.  Add each
            // value to the corresponding coordinate in the input value.
            double xDisplace = x + (this.SourceModules[1].GetValue(x, y, z));
            double yDisplace = y + (this.SourceModules[2].GetValue(x, y, z));
            double zDisplace = z + (this.SourceModules[3].GetValue(x, y, z));

            // Retrieve the output value using the offsetted input value instead of
            // the original input value.
            return this.SourceModules[0].GetValue(xDisplace, yDisplace, zDisplace);
        }
    }
}
