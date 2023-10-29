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
        catch(Exception ex)
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
            if(matchingEntry == null)
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
            if(user == null)
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

    public async Task<Exception?> InsertNewMeal(Meal meal)
    {
        try
        {
            await _context.Meals.AddAsync(meal);
            await _context.SaveChangesAsync();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
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
            if(meal == null)
            {
                return new Exception("No matching meal found");
            }
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
            if(entry == null)
            {
                return new Exception("No matching diary entry found");
            }
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
}
