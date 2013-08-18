using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using NHibernate.FlowQuery.Test.Setup.Entities;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class ExpressionHelperTest
    {
        #region Methods (28)

        [Test]
        public void CanGetConstantRootStringForReferenceType()
        {
            Expression<Func<object>> expression = () => bool.FalseString; // Non constant root

            expression = () => expression.Body; // Creating a constant root

            Assert.That(ExpressionHelper.GetConstantRootString(expression), Is.Not.Null);
        }

        [Test]
        public void CanGetConstantRootStringForValueType()
        {
            Expression<Func<object>> expression = () => bool.FalseString; // Non constant root

            expression = () => expression.ToString().Length; // Creating a constant root

            Assert.That(ExpressionHelper.GetConstantRootString(expression), Is.Not.Null);
        }

        [Test]
        public void CanGetPropertyNameFromExpressionAndExpectedRootWhenRootDoesNotMatchExpected()
        {
            UserDto x = null;

            Expression<Func<UserDto, object>> expression = u => x.Username;

            Assert.That(ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name), Is.EqualTo("x.Username"));
        }

        [Test]
        public void CanGetPropertyNameFromExpressionAndExpectedRootWhenRootMatchExpected()
        {
            Expression<Func<UserDto, object>> expression = u => u.Username;

            Assert.That(ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name), Is.EqualTo("Username"));
        }

        [Test]
        public void CanGetPropertyNameFromExpressionWithStringConstant()
        {
            Expression<Func<UserDto, object>> x = u => "u.Username";

            Assert.That(ExpressionHelper.GetPropertyName(x), Is.EqualTo("u.Username"));
        }

        [Test]
        public void CanGetPropertyNameFromMemberExpression()
        {
            Expression<Func<UserDto, object>> x = u => u.Username;

            Assert.That(ExpressionHelper.GetPropertyName(x.Body as MemberExpression), Is.EqualTo("u.Username"));
        }

        [Test]
        public void CanGetPropertyNameFromMemberExpressionWithConstantRoot()
        {
            Expression<Func<UserDto, object>> x = u => u.Username;

            x = u => x.Body;

            Assert.That(ExpressionHelper.GetPropertyName(x.Body as MemberExpression), Is.EqualTo("x.Body"));
        }

        [Test]
        public void CanGetPropertyNameFromMethodCallExpression()
        {
            Expression<Func<UserDto, object>> x = u => u.Username.GetHashCode();

            Assert.That(ExpressionHelper.GetPropertyName(x), Is.EqualTo("u.Username"));
        }

        [Test]
        public void CanGetRootForDeepExpression()
        {
            UserDto u = null;

            Expression<Func<object>> expression = () => u.Username.Length;

            Assert.That(ExpressionHelper.GetRoot(expression), Is.EqualTo("u"));
        }

        [Test]
        public void CanGetRootForDeepExpressionWithMethodCalls()
        {
            UserDto u = null;

            Expression<Func<object>> expression = () => u.Username.Length.ToString().Length;

            Assert.That(ExpressionHelper.GetRoot(expression), Is.EqualTo("u"));
        }

        [Test]
        public void CanGetRootForMethodCall()
        {
            UserDto u = null;

            Expression<Func<object>> expression = () => u.GetHashCode();

            Assert.That(ExpressionHelper.GetRoot(expression), Is.EqualTo("u"));
        }

        [Test]
        public void CanGetRootForReferenceType()
        {
            UserDto u = null;

            Expression<Func<object>> expression = () => u.Username;

            Assert.That(ExpressionHelper.GetRoot(expression), Is.EqualTo("u"));
        }

        [Test]
        public void CanGetRootForStringConstant()
        {
            Expression<Func<object>> expression = () => "u";

            Assert.That(ExpressionHelper.GetRoot(expression), Is.EqualTo("u"));
        }

        [Test]
        public void CanGetRootForValueType()
        {
            UserDto u = null;

            Expression<Func<object>> expression = () => u.IsOnline;

            Assert.That(ExpressionHelper.GetRoot(expression), Is.EqualTo("u"));
        }

        [Test]
        public void CanGetValueTyped()
        {
            Expression<Func<object>> expression = () => "Hello World";

            Assert.That(ExpressionHelper.GetValue<string>(expression.Body), Is.EqualTo("Hello World"));
        }

        [Test]
        public void CanGetValueUntyped()
        {
            Expression<Func<object>> expression = () => "Hello World";

            Assert.That(ExpressionHelper.GetValue(expression.Body), Is.EqualTo("Hello World"));
        }

        [Test]
        public void CanVerifyIfExpressionHasConstantRoot()
        {
            Expression<Func<object>> expression = () => bool.FalseString; // Non constant root

            Assert.That(ExpressionHelper.HasConstantRoot(expression), Is.False);

            expression = () => expression.Body.ToString().Length; // Creating a constant root

            Assert.That(ExpressionHelper.HasConstantRoot(expression), Is.True);
        }

        [Test]
        public void GetConstantRootStringReturnsNullIfExpressionIsNotSupported()
        {
            long x = 22;

            Expression<Func<bool>> expression = () => x == 23;

            Assert.That(ExpressionHelper.GetConstantRootString(expression), Is.Null);
        }

        [Test]
        public void IsRootedThrowsIfExpressionIsNull()
        {
            Expression<Func<UserDto, object>> expression = null;

            Assert.That(() =>
                        {
                            ExpressionHelper.IsRooted(expression, "x", null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void IsRootedDoesNotThrowIfExpectedRootIsNull()
        {
            Expression<Func<UserDto, object>> expression = x => x.Id;

            Assert.That(() =>
                        {
                            ExpressionHelper.IsRooted(expression, null, null);

                        }, Throws.Nothing);
        }

        [Test]
        public void GetValueDoesNotThrowWhereTargetInvocationExceptionMightBeExpected()
        {
            object argument = null;

            Expression<Func<object>> expression = () => argument.ToString();

            object value = 1;

            Assert.That(() =>
                        {
                            value = ExpressionHelper.GetValue(expression.Body);

                        }, Throws.Nothing);

            Assert.That(value, Is.Null);
        }

        [Test]
        public void IsRootedDoesNotThrowIfExpectedRootIsEmpty()
        {
            Expression<Func<UserDto, object>> expression = x => x.Id;

            Assert.That(() =>
                        {
                            ExpressionHelper.IsRooted(expression, string.Empty, null);

                        }, Throws.Nothing);
        }

        [Test]
        public void GetPropertyNameFromExpressionAndExpectedRootDoesNotThrowWhenExpectedIsEmpty()
        {
            Expression<Func<UserDto, object>> expression = u => u.Username;

            Assert.That(() =>
                        {
                            ExpressionHelper.GetPropertyName(expression.Body, string.Empty);

                        }, Throws.Nothing);
        }

        [Test]
        public void GetPropertyNameFromExpressionAndExpectedRootDoesNotThrowWhenExpectedIsNull()
        {
            Expression<Func<UserDto, object>> expression = u => u.Username;

            Assert.That(() =>
                        {
                            ExpressionHelper.GetPropertyName(expression.Body, null);

                        }, Throws.Nothing);
        }

        [Test]
        public void GetPropertyNameFromExpressionAndExpectedRootThrowsWhenExpressionIsNull()
        {
            Assert.That(() =>
                        {
                            ExpressionHelper.GetPropertyName(null, "u");

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetPropertyNameFromExpressionThrowsWhenExpressionIsNotSupported()
        {
            Expression<Func<object>> x = () => true;

            Assert.That(() => { ExpressionHelper.GetPropertyName(x); }, Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void GetPropertyNameFromExpressionThrowsWhenExpressionIsNull()
        {
            Expression<Func<UserDto, object>> x = null;

            Assert.That(() => { ExpressionHelper.GetPropertyName(x); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetPropertyNameFromMemberExpressionThrowsIfExpressionIsNull()
        {
            MemberExpression x = null;

            Assert.That(() => { ExpressionHelper.GetPropertyName(x); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetRootReturnsNullIfExpressionTypeIsNotSupported()
        {
            Expression<Func<object>> expression = () => true;

            Assert.That(ExpressionHelper.GetRoot(expression), Is.Null);
        }

        [Test]
        public void GetRootThrowsIfExpressionIsNull()
        {
            Assert.That(() => { ExpressionHelper.GetRoot(null); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetValueThrowsIfExpressionIsNull()
        {
            Assert.That(() => { ExpressionHelper.GetValue(null); }, Throws.InstanceOf<ArgumentNullException>());
            Assert.That(() => { ExpressionHelper.GetValue<string>(null); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void HasConstantRootReturnsFalseWhenProvidedWithNull()
        {
            Assert.That(ExpressionHelper.HasConstantRoot(null), Is.False);
        }

        [Test]
        public void CombineThrowsIfExpressionsIsNull()
        {
            Assert.That(() =>
                        {
                            ExpressionHelper.Combine<UserEntity, UserDto>(null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void CombineThrowsNothingIfExpressionsContainsNull()
        {
            Assert.That(() =>
                        {
                            ExpressionHelper.Combine<UserEntity, UserDto>(x => new UserDto() { Id = x.Id }, null, x => new UserDto() { Username = x.Username });

                        }, Throws.Nothing);
        }

        [Test]
        public void CombineThrowsIfExpressionsContainsNonMemberInitExpression()
        {
            Assert.That(() =>
                        {
                            UserDto dto = new UserDto();

                            ExpressionHelper.Combine<UserEntity, UserDto>(x => new UserDto() { Id = x.Id }, x => dto, x => new UserDto() { Username = x.Username });

                        }, Throws.ArgumentException);
        }

        [Test]
        public void CombineThrowsIfFirstExpressionIsOtherThanMemberInitExpressionOrNewExpression()
        {
            Assert.That(() =>
            {
                UserDto dto = new UserDto();

                ExpressionHelper.Combine<UserEntity, UserDto>(x => dto, x => new UserDto() { Username = x.Username });

            }, Throws.ArgumentException);
        }

        #endregion Methods
    }
}