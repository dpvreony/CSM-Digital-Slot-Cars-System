using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using AutoMapper;

namespace SlotCarsGo_Server.Tests
{
    [TestClass]
    public class DataTransferObjectTests
    {
        static bool automapperIntialised = false;
        // Setup
        ApplicationUser user;
        Car car;
        Driver driver;
        DriverResult driverResult;
        LapTime laptime;
        RaceSession raceSession;
        RaceType raceType;
        Track track;

        int carId = 5;
        int driverId = 7;
        int driverResultId = 124632;
        int trackId = 45;
        int raceSessionId = 23564;
        int raceTypeId = 3;

        string name = "Test Car Name";
        TimeSpan trackRecord = new TimeSpan(0, 0, 0, 5, 450);
        string recordHolderName = "Test User";
        string imageName = "image.png";
        int controllerId = 5;
        float trackLength = 7.34f;
        string trackName = "Test GP";
        bool finished = true;
        float fuel = 0.12f;
        int crashPenalty = 3;
        DateTime endTime = DateTime.Now.AddMinutes(25);
        DateTime startTime = DateTime.Now;
        bool fuelEnabled = false;
        bool lapsNotDuration = false;
        int numDrivers = 4;
        TimeSpan raceLength = new TimeSpan(0,25,0);
        int raceLimitValue = 25;
        string raceTypeName = "Test Race Type";
        string raceTypeRules = "Test rules";
        string raceTypeSymbol = "E234";
        int lapNumber = 24;


        [TestInitialize()]
        public void Initialize()
        {
            if (!automapperIntialised)
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<RaceSession, RaceSessionDTO>();
                    cfg.CreateMap<DriverResult, DriverResultDTO>()
                        .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.ApplicationUserId))
                        .ReverseMap()
                            .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.DriverId));
                    cfg.CreateMap<Track, TrackDTO>();
                    cfg.CreateMap<Car, CarDTO>()
                        .ForMember(dest => dest.RecordHolder, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                        .ReverseMap()
                            .ForMember(dest => dest.ApplicationUser.UserName, opt => opt.MapFrom(src => src.RecordHolder));
                    cfg.CreateMap<RaceType, RaceTypeDTO>();
                    cfg.CreateMap<Driver, DriverDTO>()
                        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ApplicationUser.Id))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                        .ForMember(dest => dest.SelectedCar, opt => opt.MapFrom(src => src.Car))
                        .ReverseMap()
                            .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.UserId))
                            .ForMember(dest => dest.ApplicationUser.UserName, opt => opt.MapFrom(src => src.UserName))
                            .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.SelectedCar));
                    cfg.CreateMap<LapTime, LapTimeDTO>();
                });

                automapperIntialised = true;
            }


            raceType = new RaceType();
            raceType.Id = raceTypeId;
            raceType.Name = raceTypeName;
            raceType.Rules = raceTypeRules;
            raceType.Symbol = raceTypeSymbol;

            user = new ApplicationUser();
            user.Id = driverId.ToString();
            user.UserName = recordHolderName;

            track = new Track();
            track.ApplicationUserId = Convert.ToInt32(user.Id);
            track.Length = trackLength;
            track.Name = trackName;
            track.Id = trackId;

            car = new Car();
            car.Id = carId;
            car.TrackId = trackId;
            car.ImageName = imageName;
            car.Name = name;
            car.Track = new Track();
            car.TrackRecord = trackRecord;
            car.ApplicationUser = user;
            car.ApplicationUser.UserName = user.UserName;

            driver = new Driver();
            driver.Id = driverId;
            driver.ApplicationUser = user;
            driver.ApplicationUserId = Convert.ToInt32(user.Id);
            driver.Car = car;
            driver.CarId = car.Id;
            driver.ControllerId = controllerId;
            driver.Track = track;
            driver.TrackId = track.Id;

            driverResult = new DriverResult();
            driverResult.Id = driverResultId;
            driverResult.ApplicationUser = user;
            driverResult.ApplicationUserId = Convert.ToInt32(user.Id);
            driverResult.BestLapTime = trackRecord;
            driverResult.Car = car;
            driverResult.CarId = car.Id;
            driverResult.ControllerNumber = controllerId;
            driverResult.Finished = finished;
            driverResult.Fuel = fuel;

            raceSession = new RaceSession();
            raceSession.Id = raceSessionId;
            raceSession.CrashPenalty = crashPenalty;
            raceSession.EndTime = endTime;
            raceSession.FuelEnabled = fuelEnabled;
            raceSession.LapsNotDuration = lapsNotDuration;
            raceSession.NumberOfDrivers = numDrivers;
            raceSession.RaceLength = raceLength;
            raceSession.RaceLimitValue = raceLimitValue;
            raceSession.RaceType = raceType;
            raceSession.RaceTypeId = raceType.Id;
            raceSession.StartTime = startTime;
            raceSession.Track = track;
            raceSession.TrackID = trackId;

            laptime = new LapTime();
            laptime.Id = 1;
            laptime.Driver = driver;
            laptime.DriverId = driverId;
            laptime.LapNumber = lapNumber;
            laptime.RaceSession = raceSession;
            laptime.RaceSessionId = raceSessionId;
            laptime.Time = trackRecord;
        }
        
        [TestMethod]
        public void CarDTO_Test()
        {
            // Setup

            // Act
            CarDTO carDTOResult = Mapper.Map<CarDTO>(car);

            // Assert
            Assert.AreEqual(car.Id, carDTOResult.Id);
            Assert.AreEqual(car.ImageName, carDTOResult.ImageName);
            Assert.AreEqual(car.Name, carDTOResult.Name);
            Assert.AreEqual(car.ApplicationUser.UserName, carDTOResult.RecordHolder);
            Assert.AreEqual(car.TrackRecord, carDTOResult.TrackRecord);
        }

        [TestMethod]
        public void DriverDTO_Test()
        {
            // Act
            DriverDTO driverDTOResult = Mapper.Map<DriverDTO>(driver);
            CarDTO carDTO = Mapper.Map<CarDTO>(driver.Car);

            //Assert
            Assert.AreEqual(driver.Id, driverDTOResult.UserId);
            Assert.AreEqual(driver.ControllerId, driverDTOResult.ControllerId);
            Assert.AreEqual(driver.ApplicationUser.ImageName, driverDTOResult.ImageName);
            Assert.AreEqual(carDTO.Id, driverDTOResult.SelectedCar.Id);
            Assert.AreEqual(carDTO.RecordHolder, driverDTOResult.SelectedCar.RecordHolder);
            Assert.AreEqual(carDTO.ImageName, driverDTOResult.SelectedCar.ImageName);
            Assert.AreEqual(carDTO.TrackRecord, driverDTOResult.SelectedCar.TrackRecord);
            Assert.AreEqual(driver.ApplicationUser.UserName, driverDTOResult.UserName);
        }

        [TestMethod]
        public void DriverResultDTO_Test()
        {
            // Act
            DriverResultDTO driverResultDTO = Mapper.Map<DriverResultDTO>(driverResult);

            // Assert
            Assert.AreEqual(driverResult.BestLapTime, driverResultDTO.BestLapTime);
            Assert.AreEqual(driverResult.CarId, driverResultDTO.CarId);
            Assert.AreEqual(driverResult.ControllerNumber, driverResultDTO.ControllerNumber);
            Assert.AreEqual(driverResult.ApplicationUserId, driverResultDTO.DriverId);
            Assert.AreEqual(driverResult.Finished, driverResultDTO.Finished);
            Assert.AreEqual(driverResult.Fuel, driverResultDTO.Fuel);
            Assert.AreEqual(driverResult.Laps, driverResultDTO.Laps);
            Assert.AreEqual(driverResult.Position, driverResultDTO.Position);
            Assert.AreEqual(driverResult.SessionId, driverResultDTO.RaceSessionId);
            Assert.AreEqual(driverResult.TimeOffPace, driverResultDTO.TimeOffPace);
            Assert.AreEqual(driverResult.TotalTime, driverResultDTO.TotalTime);
        }

        [TestMethod]
        public void LapTimeDTO_Test()
        {
            LapTimeDTO laptimeDTO = Mapper.Map<LapTimeDTO>(laptime);

            Assert.AreEqual(laptime.DriverId, laptimeDTO.DriverId);
            Assert.AreEqual(laptime.Id, laptimeDTO.Id);
            Assert.AreEqual(laptime.LapNumber, laptimeDTO.LapNumber);
            Assert.AreEqual(laptime.RaceSessionId, laptimeDTO.RaceSessionId);
            Assert.AreEqual(laptime.Time, laptimeDTO.Time);
        }

        [TestMethod]
        public void RaceSessionDTO_Test()
        {
            // Act 
            RaceSessionDTO raceSessionDTO = Mapper.Map<RaceSessionDTO>(raceSession);

            //Assert
            Assert.AreEqual(raceSession.EndTime, raceSessionDTO.EndTime);
            Assert.AreEqual(raceSession.Id, raceSessionDTO.Id);
            Assert.AreEqual(raceSession.NumberOfDrivers, raceSessionDTO.NumberOfDrivers);
            Assert.AreEqual(raceSession.RaceTypeId, raceSessionDTO.RaceType.Id);
            Assert.AreEqual(raceSession.RaceType.Name, raceSessionDTO.RaceType.Name);
            Assert.AreEqual(raceSession.StartTime, raceSessionDTO.StartTime);
            Assert.AreEqual(raceSession.TrackID, raceSessionDTO.TrackId);
        }

        [TestMethod]
        public void RaceTypeDTO_Test()
        {
            // Act
            RaceTypeDTO raceTypeDTO = Mapper.Map<RaceTypeDTO>(raceType);

            // Assert
            Assert.AreEqual(raceType.Id, raceTypeDTO.Id);
            Assert.AreEqual(raceType.Name, raceTypeDTO.Name);
            Assert.AreEqual(raceType.Rules, raceTypeDTO.Rules);
            Assert.AreEqual(raceType.Symbol, raceTypeDTO.Symbol);
        }

        [TestMethod]
        public void TrackDTO_Test()
        {
            // Act
            TrackDTO trackDTO = Mapper.Map<TrackDTO>(track);

            // Assert
            Assert.AreEqual(track.Id, trackDTO.Id);
            Assert.AreEqual(track.Length, trackDTO.Length);
            Assert.AreEqual(track.Name, trackDTO.Name);
        }
    }
}
