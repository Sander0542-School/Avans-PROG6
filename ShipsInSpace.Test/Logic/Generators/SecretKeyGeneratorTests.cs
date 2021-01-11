using ShipsInSpace.Logic.Generators;
using Xunit;

namespace ShipsInSpace.Test.Logic.Generators
{
    public class SecretKeyGeneratorTests
    {
        [Fact]
        public void Key_Length()
        {
            var secretKey = new SecretKeyGenerator().Generate();
            
            Assert.Equal(SecretKeyGenerator.Length, secretKey.Length);
        }
    }
}