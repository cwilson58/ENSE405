using EatingHabitAnalyzerAPI.Models;
using System.Runtime.CompilerServices;

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
    public Task<List<Meal>> GetMealListByEntryId(int entryId);
    public Task<Exception?> InsertNewDiaryEntry(DiaryEntry diaryEntry);
    public Task<Exception?> UpdateDiaryEntry(DiaryEntry diaryEntry);
    public Task<Exception?> DeleteDiaryEntry(int entryId);
    public Task<DiaryEntry?> GetDiaryEntryById(int id);
    public Task<DiaryEntry?> GetDirayEntryByDateAndUserId(DateTime date, int userId);
    public Task<MealFood?> GetMealFoodByIdAndBarcode(int mealId, string barcode);
    public Task<MealFood?> GetMealFoodById(int mealFoodId);
    public Task<List<MealFood>> GetMealFoodListByMealId(int mealId);
    public Task<Exception?> InsertNewMealFood(MealFood mealFood);
    public Task<Exception?> DeleteMealFood(MealFood mealFood);
    public Task<Exception?> UpdateMealFood(MealFood mealFood);

    public Task<Exception?> InsertNewGroup(Group group);

    public Task<Exception?> InsertNewGroupMember(GroupMember groupMember);

    public Task<Exception?> InsertNewGroupGoal(Goal goal);

    public Task<Exception?> InsertNewGoalEntrie(GoalEntry goalEntry);

    public Task<Group?> GetGroupById(int id);

    public Task<List<GroupMember>> GetGroupMembersById(int id);

    public Task<Goal?> GetGoalById(int id);

    public Task<List<Goal>> GetGoalsByGroupId(int id);

    public Task<GoalEntry?> GetGoalEntryById(int id);

    public Task<List<Group>> GetGroupsByUserId(int id);

    public Task<List<GroupMember>> GetGroupMembersByUserId(int id);

    public Task<List<GoalEntry>> GetGoalEntriesByUserId(int id);

    public Task<Exception?> CreateNewExerciseLog(ExerciseLog exerciseLog);

    public Task<ExerciseLog?> GetExerciseLogById(int id);

    public Task<ExerciseLog?> GetExerciseLogByDate(DateTime date, int userId);
}
