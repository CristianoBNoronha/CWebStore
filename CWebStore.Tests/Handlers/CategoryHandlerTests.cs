﻿using CWebStore.Domain.Commands;
using CWebStore.Domain.Handlers;
using CWebStore.Domain.Repositories.Interfaces;
using CWebStore.Shared.Interfaces;
using Flunt.Notifications;
using Moq;

namespace CWebStore.Tests.Handlers;

[TestClass]
public class CategoryHandlerTests
{
    private readonly Mock<CreateCategoryCommand> _command;

    private readonly Mock<ICategoryRepository> _repositoryMock;

    private readonly Mock<Category> _categoryMock;

    public CategoryHandlerTests()
    {
        _command = new Mock<CreateCategoryCommand>();
        _repositoryMock = new Mock<ICategoryRepository>();
        _categoryMock = new Mock<Category>();
    }

    [TestMethod]
    [TestCategory("CWebStore.Domain.Handlers")]
    public void Given_an_invalid_category_name_CategoryHandler_should_return_CommandResult_error_message()
    {
        var handler = new CategoryHandler(_repositoryMock.Object);
        var command = new CreateCategoryCommand(string.Empty);
        var handlerResult = handler.Handle(command) as CommandResult;

        var error = "Category name must not be null or empty.";
        var message = "This is not a valid Category.";
        Assert.AreEqual(error, handlerResult.Errors.First().Message);
        Assert.AreEqual(message, handlerResult.Message);
    }
    
    [TestMethod]
    [TestCategory("CWebStore.Domain.Handlers")]
    public void Given_already_existing_category_CategoryHandler_should_return_CommandResult_error_messages_and_list()
    {
        _repositoryMock.Setup(x => x.CategoryExists("Category name")).Returns(true);
        var command = new CreateCategoryCommand("Category name");
        var handler = new CategoryHandler(_repositoryMock.Object);
        var result = handler.Handle(command) as CommandResult;

        var message = "This category already exists.";
        Assert.AreEqual(message, result.Message);
    }

    [TestMethod]
    [TestCategory("CWebStore.Domain.Handlers")]
    public void Given_categoryCommand_CategoryHandler_should_return_CommandResult_success_message()
    {
        var command = new CreateCategoryCommand("Category name");
        _repositoryMock.Setup(x => x.CategoryExists("Category name")).Returns(false);
        
        var handler = new CategoryHandler(_repositoryMock.Object);
        var result = handler.Handle(command) as CommandResult;

        var message = "Category successfully saved.";
        Assert.AreEqual(message, result.Message);
    }
}