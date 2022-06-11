using OpenTK.Graphics.OpenGL;

namespace Collision_Simulation
{

    public readonly struct ShaderUniform
    {
        public readonly string Name;
        public readonly int Location;
        public readonly ActiveUniformType Type;

        public ShaderUniform(string name, int location, ActiveUniformType type)
        {
            this.Name = name;
            this.Location = location;
            this.Type = type;
        }
    }

    public readonly struct ShaderAttribute
    {
        public readonly string Name;
        public readonly int Location;
        public readonly ActiveAttribType Type;

        public ShaderAttribute(string name, int location, ActiveAttribType type)
        {
            this.Name = name;
            this.Location = location;
            this.Type = type;
        }
    }


    public sealed class ShaderProgram : IDisposable
    {
        private bool disposed;
        public readonly int ShaderProgramHandle;
        public readonly int VertexShaderHandle;
        public readonly int FragmentShaderHandle;

        private readonly ShaderUniform[] uniforms;
        private readonly ShaderAttribute[] attributes;

        public ShaderProgram(string vertexShaderLocation, string fragmentShaderLocation)
        {
            this.disposed = false;

            if(!ShaderProgram.CompileVertexShader(vertexShaderLocation, out this.VertexShaderHandle, out string vertexShaderCompileError))
            {
                throw new ArgumentException(vertexShaderCompileError);
            }

            if (!ShaderProgram.CompileFragmentShader(fragmentShaderLocation, out this.FragmentShaderHandle, out string fragmentShaderCompileError))
            {
                throw new ArgumentException(fragmentShaderCompileError);
            }

            this.ShaderProgramHandle = ShaderProgram.CreateLinkProgram(VertexShaderHandle, FragmentShaderHandle);

            this.uniforms = ShaderProgram.CreateUniformList(this.ShaderProgramHandle);
            this.attributes = ShaderProgram.CreateAttributeList(this.ShaderProgramHandle);
        }

        ~ShaderProgram()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            GL.DeleteShader(this.VertexShaderHandle);
            GL.DeleteShader(this.FragmentShaderHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(this.ShaderProgramHandle);


            this.disposed = true;
            GC.SuppressFinalize(this);
        }

        public ShaderUniform[] GetUniformList()
        {
            ShaderUniform[] result = new ShaderUniform[this.uniforms.Length];
            Array.Copy(this.uniforms, result, this.uniforms.Length);
            return result;
        }

        public void setUniform(string name, float v1)
        {
            if(!this.GetShaderUniform(name, out ShaderUniform uniform))
            {
                throw new ArgumentException("Name was not found.");
            }

            if(uniform.Type != ActiveUniformType.Float)
            {
                throw new ArgumentException("Uniform type is not a Float.");
            }

            GL.UseProgram(this.ShaderProgramHandle);
            GL.Uniform1(uniform.Location, v1);
            GL.UseProgram(0);
        }

        public void setUniform(string name, float v1, float v2)
        {
            if (!this.GetShaderUniform(name, out ShaderUniform uniform))
            {
                throw new ArgumentException("Name was not found.");
            }

            if (uniform.Type != ActiveUniformType.FloatVec2)
            {
                throw new ArgumentException("Uniform type is not a FloatVec2.");
            }

            GL.UseProgram(this.ShaderProgramHandle);
            GL.Uniform2(uniform.Location, v1, v2);
            GL.UseProgram(0);
        }

        public ShaderAttribute[] GetAttributeList()
        {
            ShaderAttribute[] result = new ShaderAttribute[this.attributes.Length];
            Array.Copy(this.attributes, result, this.attributes.Length);
            return result;
        }

        private bool GetShaderUniform(string name, out ShaderUniform uniform)
        {
            uniform = new ShaderUniform();

            for (int i = 0; i < this.uniforms.Length; i++)
            {
                uniform = this.uniforms[i];

                if(name == uniform.Name)
                {
                    return true;
                }
            }

            return false;
        }

        private bool GetShaderAttribute(string name, out ShaderAttribute attribute)
        {
            attribute = new ShaderAttribute();

            for (int i = 0; i < this.attributes.Length; i++)
            {
                attribute = this.attributes[i];

                if (name == attribute.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CompileVertexShader(string vertexShaderLocation, out int vertexShaderHandle, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Compile Shaders
            vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, File.ReadAllText(vertexShaderLocation));
            GL.CompileShader(vertexShaderHandle);

            // VertexShader Error Log
            string vertexShaderInfo = GL.GetShaderInfoLog(vertexShaderHandle);
            if (vertexShaderInfo != String.Empty)
            {
                errorMessage = vertexShaderInfo;
                return false;
            }

            return true;
        }

        public static bool CompileFragmentShader(string fragmentShaderLocation, out int fragmentShaderHandle, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Compile Shaders
            fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, File.ReadAllText(fragmentShaderLocation));
            GL.CompileShader(fragmentShaderHandle);

            // FragmentShader Error Log
            string fragmentShaderInfo = GL.GetShaderInfoLog(fragmentShaderHandle);
            if (fragmentShaderInfo != String.Empty)
            {
                errorMessage = fragmentShaderInfo;
                return false;
            }

            return true;
        }


        public static int CreateLinkProgram(int vertexShaderHandle, int fragmentShaderHandle)
        {
            int shaderProgramHandle = GL.CreateProgram();

            GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);

            GL.LinkProgram(shaderProgramHandle);

            GL.DetachShader(shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(shaderProgramHandle, fragmentShaderHandle);

            return shaderProgramHandle;
        }

        public static ShaderUniform[] CreateUniformList(int shaderProgramHandle)
        {
            GL.GetProgram(shaderProgramHandle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

            ShaderUniform[] uniforms = new ShaderUniform[uniformCount];

            for(int i = 0; i < uniformCount; i++)
            {
                GL.GetActiveUniform(shaderProgramHandle, i, 256, out _, out _, out ActiveUniformType type, out string name);
                int location = GL.GetUniformLocation(shaderProgramHandle, name);

                uniforms[i] = new ShaderUniform(name, location, type);
            }

            return uniforms;
        }

        public static ShaderAttribute[] CreateAttributeList(int shaderProgramHandle)
        {
            GL.GetProgram(shaderProgramHandle, GetProgramParameterName.ActiveAttributes, out int attributeCount);

            ShaderAttribute[] attributes = new ShaderAttribute[attributeCount];

            for (int i = 0; i < attributeCount; i++)
            {
                GL.GetActiveAttrib(shaderProgramHandle, i, 256, out _, out _, out ActiveAttribType type, out string name);
                int location = GL.GetAttribLocation(shaderProgramHandle, name);

                attributes[i] = new ShaderAttribute(name, location, type);
            }

            return attributes;
        }
    }
}
