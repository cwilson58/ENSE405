using EatingHabitAnalyzerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EatingHabitAnalyzerAPI.Service;

public class DatabaseService : IDatabaseService
{
    private readonly EatingHabitAnalyzerContext _context;

    public DatabaseService(EatingHabitAnalyzerContext context)
    {
        _context = context;
    }

    public async Task<List<Food>> GetFoodListAsync(List<string> barcodes)
    {
        return await _context.Foods.Where(food => barcodes.Contains(food.Barcode)).ToListAsync();
    }

    public async Task<Food?> GetFoodAsync(string barcode)
    {
        //TODO: GO TO OPEN FOOD FACTS API IF NOT FOUND
        return await _context.Foods.Where(food => food.Barcode == barcode).FirstOrDefaultAsync();
    }

    public async Task<Exception?> InsertNewFood(Food food)
    {
        try
        {
            await _context.Foods.AddAsync(food);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> InsertFoods(List<Food> foods)
    {
        try
        {
            await _context.Foods.AddRangeAsync(foods);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }

    }

    public async Task<Exception?> UpdateFood(Food food)
    {
        try
        {
            _context.Foods.Update(food);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> DeleteFood(string barcode)
    {
        try
        {
            var matchingEntry = await _context.Foods.Where(food => food.Barcode == barcode).FirstOrDefaultAsync();
            if (matchingEntry == null)
            {
                return new Exception("No matching food found");
            }
            _context.Foods.Remove(matchingEntry);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<User?> GetUserAsync(string email)
    {
        try
        {
            return await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Exception?> InsertNewUser(User user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> UpdateUser(User user)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> DeleteUser(string email)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
            if (user == null)
            {
                return new Exception("No matching user found");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<int?> InsertNewMeal(Meal meal)
    {
        try
        {
            await _context.Meals.AddAsync(meal);
            await _context.SaveChangesAsync();
            return meal.MealId;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Exception?> UpdateMeal(Meal meal)
    {
        try
        {
            _context.Meals.Update(meal);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> DeleteMeal(int mealId)
    {
        try
        {
            var meal = await _context.Meals.FirstOrDefaultAsync(meal => meal.MealId == mealId);
            if (meal == null)
            {
                return new Exception("No matching meal found");
            }
            _context.MealFoods.RemoveRange(_context.MealFoods.Where(x => x.MealId == mealId));
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Meal?> GetMealById(int id)
    {
        return await _context.Meals.Where(meal => meal.MealId == id).FirstOrDefaultAsync();
    }

    public async Task<List<Meal>> GetMealListByEntryId(int entryId)
    {
        return await _context.Meals.Where(meal => meal.EntryId == entryId).ToListAsync();
    }

    public async Task<Exception?> InsertNewDiaryEntry(DiaryEntry diaryEntry)
    {
        try
        {
            await _context.DiaryEntries.AddAsync(diaryEntry);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> UpdateDiaryEntry(DiaryEntry diaryEntry)
    {
        try
        {
            _context.DiaryEntries.Update(diaryEntry);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> DeleteDiaryEntry(int entryId)
    {
        try
        {
            var entry = await _context.DiaryEntries.FirstOrDefaultAsync(entry => entry.EntryId == entryId);
            if (entry == null)
            {
                return new Exception("No matching diary entry found");
            }
            var meals = _context.Meals.Where(x => x.EntryId == entryId);
            var mealIdSet = meals.Select(x => x.MealId).ToList();
            _context.MealFoods.RemoveRange(_context.MealFoods.Where(x => mealIdSet.Contains(x.MealId!.Value)));
            _context.Meals.RemoveRange(meals);
            _context.DiaryEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<DiaryEntry?> GetDiaryEntryById(int id)
    {
        return await _context.DiaryEntries.FirstOrDefaultAsync(entry => entry.EntryId == id);
    }

    public async Task<DiaryEntry?> GetDirayEntryByDateAndUserId(DateTime date, int userId)
    {
        return await _context.DiaryEntries.FirstOrDefaultAsync(entry => entry.UserId == userId && entry.EntryDateTime == date);
    }

    public async Task<MealFood?> GetMealFoodByIdAndBarcode(int mealId, string barcode)
    {
        return await _context.MealFoods.FirstOrDefaultAsync(x => x.MealId == mealId && x.Barcode == barcode);
    }

    public async Task<MealFood?> GetMealFoodById(int mealFoodId)
    {
        return await _context.MealFoods.FirstOrDefaultAsync(x => x.MealFoodId == mealFoodId);
    }
    public async Task<Exception?> InsertNewMealFood(MealFood mealFood)
    {
        try
        {
            await _context.MealFoods.AddAsync(mealFood);
            await _context.SaveChangesAsync();
            return null;
        }
        catch(Exception ex)
        {
            return ex;
        }
    }
    public async Task<Exception?> DeleteMealFood(MealFood mealFood)
    {
        try
        {
            _context.MealFoods.Remove(mealFood);
            await _context.SaveChangesAsync();
            return null;
        }
        catch(Exception ex)
        {
            return ex;
        }
    }
    public async Task<Exception?> UpdateMealFood(MealFood mealFood)
    {
        try
        {
            _context.MealFoods.Update(mealFood);
            await _context.SaveChangesAsync();
            return null;
        }
        catch(Exception ex)
        {
            return ex;
        }
    }

    public async Task<List<MealFood>> GetMealFoodListByMealId(int mealId)
    {
        var meals = _context.MealFoods.Where(x => x.MealId == mealId).ToList();
        meals.ForEach(x => x.Food = _context.Foods.First(y => y.Barcode == x.Barcode));
        return meals;
    }

    public async Task<Exception?> InsertNewGroup(Group group)
    {
        try
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return null;
        } 
        catch(Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> InsertNewGroupMember(GroupMember groupMember)
    {
        try
        {
            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();
            return null;
        }
        catch(Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> InsertNewGroupGoal(Goal goal)
    {
        try
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
            return null;
        }
        catch(Exception ex)
        {
            return ex;
        }
    }

    public async Task<Exception?> InsertNewGoalEntrie(GoalEntry goalEntry)
    {
        try
        {
            _context.GoalEntries.Add(goalEntry);
            await _context.SaveChangesAsync();
            return null;
        }
        catch(Exception ex)
        {
            return ex;
        }
    }

    public Task<Group?> GetGroupById(int id)
    {
        return _context.Groups.FirstOrDefaultAsync(x => x.GroupID == id);
    }

    public Task<List<GroupMember>> GetGroupMembersById(int id)
    {
        return _context.GroupMembers.Where(x => x.GroupID == id).ToListAsync();
    }

    public Task<Goal?> GetGoalById(int id)
    {
        return _context.Goals.FirstOrDefaultAsync(x => x.GoalID == id);
    }

    public Task<GoalEntry?> GetGoalEntryById(int id)
    {
        return _context.GoalEntries.FirstOrDefaultAsync(x => x.GoalEntryID == id);
    }

    public Task<List<Group>> GetGroupsByUserId(int id)
    {
        var groupIds = _context.GroupMembers.Where(x => x.UserID == id).Select(x => x.GroupID).ToList();
        var ownedGroups = _context.Groups.Where(x => x.OwnerID == id).Select(x => x.GroupID).ToList();
        groupIds.AddRange(ownedGroups);
        return _context.Groups.Where(y => groupIds.Contains(y.GroupID)).ToListAsync();
    }

    public Task<List<GroupMember>> GetGroupMembersByUserId(int id)
    {
        return _context.GroupMembers.Where(x => x.UserID == id).ToListAsync();
    }

    public Task<List<GoalEntry>> GetGoalEntriesByUserId(int id)
    {
        return _context.GoalEntries.Where(x => x.UserID == id).ToListAsync();
    }

    public async Task<Exception?> CreateNewExerciseLog(ExerciseLog exerciseLog)
    {
        try
        {
            _context.ExerciseLogs.Add(exerciseLog);
            await _context.SaveChangesAsync();
            return null;
        }
        catch(Exception ex)
        {
            return ex;
        }
    }

    public Task<ExerciseLog?> GetExerciseLogById(int id)
    {
        return _context.ExerciseLogs.FirstOrDefaultAsync(x => x.LogID == id);
    }

    public Task<ExerciseLog?> GetExerciseLogByDate(DateTime date, int userId)
    {
        return _context.ExerciseLogs.FirstOrDefaultAsync(x => x.LogDate == date && x.UserID == userId);
    }

}
