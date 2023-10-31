using EatingHabitAnalyzerAPI.Models;

namespace EatingHabitAnalyzerAPI.Service;

public interface IDatabaseService
{
    //CRUD Tasks for tables
    public Task<List<Food>> GetFoodListAsync(List<string> barcodes);
    public Task<Food?> GetFoodAsync(string barcode);
    public Task<Exception?> InsertNewFood(Food food);
    public Task<Exception?> InsertFoods(List<Food> foods);
    public Task<Exception?> UpdateFood(Food food);
    public Task<Exception?> DeleteFood(string barcode);
    public Task<User?> GetUserAsync(string email);
    public Task<Exception?> InsertNewUser(User user);
    public Task<Exception?> UpdateUser(User user);
    public Task<Exception?> DeleteUser(string email);
    public Task<int?> InsertNewMeal(Meal meal);
    public Task<Exception?> UpdateMeal(Meal meal);
    public Task<Exception?> DeleteMeal(int mealId);
    public Task<Meal?> GetMealById(int id);
    public Task<Exception?> InsertNewDiaryEntry(DiaryEntry diaryEntry);
    public Task<Exception?> UpdateDiaryEntry(DiaryEntry diaryEntry);
    public Task<Exception?> DeleteDiaryEntry(int entryId);
    public Task<DiaryEntry?> GetDiaryEntryById(int id);
    public Task<DiaryEntry?> GetDirayEntryByDateAndUserId(DateTime date, int userId);
    public Task<MealFood?> GetMealFoodByIdAndBarcode(int mealId, string barcode);
    public Task<MealFood?> GetMealFoodById(int mealFoodId);
    public Task<Exception?> InsertNewMealFood(MealFood mealFood);
    public Task<Exception?> DeleteMealFood(MealFood mealFood);
    public Task<Exception?> UpdateMealFood(MealFood mealFood);

    //TODO CRUD Tasks for Feeling


}
