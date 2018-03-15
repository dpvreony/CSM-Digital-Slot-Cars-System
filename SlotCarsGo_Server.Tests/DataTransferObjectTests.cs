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
        BestLapTime bestLapTime;

        string carId = "5";
        string driverId = "7";
        string driverResultId = "124632";
        string trackId = "45";
        string raceSessionId = "23564";
        string raceTypeId = "3";
        string lapTimeId = "125678";
        string bestLapTimeId = "123";

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
                    cfg.CreateMap<RaceSession, RaceSessionDTO>()
                        .ReverseMap()
                            .ForMember(src => src.DriverResults, opt => opt.Ignore())
                            .ForMember(src => src.RaceType, opt => opt.Ignore())
                            .ForMember(src => src.Track, opt => opt.Ignore());
                    cfg.CreateMap<DriverResult, DriverResultDTO>()
                        .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.ApplicationUserId))
                        .ReverseMap()
                            .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.DriverId));
                    cfg.CreateMap<Track, TrackDTO>()
                        .ReverseMap()
                            .ForMember(src => src.BestLapTimeId, opt => opt.Ignore())
                            .ForMember(src => src.ApplicationUsers, opt => opt.Ignore())
                            .ForMember(src => src.Cars, opt => opt.Ignore());
                    cfg.CreateMap<Car, CarDTO>()
                        .ForPath(dest => dest.RecordHolder, opt => opt.MapFrom(src => src.BestLapTime.ApplicationUser.UserName))
                        .ForPath(dest => dest.TrackRecord, opt => opt.MapFrom(src => src.BestLapTime.LapTime.Time))
                        .ReverseMap()
                            .ForPath(src => src.BestLapTimeId, opt => opt.Ignore());
                    cfg.CreateMap<RaceType, RaceTypeDTO>();
                    cfg.CreateMap<Driver, DriverDTO>()
                        .ForPath(dest => dest.UserId, opt => opt.MapFrom(src => src.ApplicationUser.Id))
                        .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                        .ForPath(dest => dest.ImageName, opt => opt.MapFrom(src => src.ApplicationUser.ImageName))
                        .ForMember(dest => dest.SelectedCar, opt => opt.MapFrom(src => src.Car))
                        .ReverseMap()
                            .ForMember(src => src.ApplicationUserId, opt => opt.MapFrom(dest => dest.UserId))
                            .ForPath(src => src.ApplicationUser.UserName, opt => opt.MapFrom(dest => dest.UserName))
                            .ForPath(src => src.ApplicationUser.ImageName, opt => opt.MapFrom(dest => dest.ImageName))
                            .ForMember(src => src.Car, opt => opt.MapFrom(dest => dest.SelectedCar));
                    cfg.CreateMap<LapTime, LapTimeDTO>()
                        .ReverseMap()
                            .ForMember(src => src.DriverResult, opt => opt.Ignore());
                });

                automapperIntialised = true;
            }

            raceType = new RaceType();
            raceType.Id = raceTypeId;
            raceType.Name = raceTypeName;
            raceType.Rules = raceTypeRules;
            raceType.Symbol = raceTypeSymbol;

            bestLapTime = new BestLapTime();
            bestLapTime.ApplicationUserId = driverId.ToString();
            bestLapTime.CarId = carId;
            bestLapTime.LapTimeId = bestLapTimeId;

            user = new ApplicationUser();
            user.Id = driverId.ToString();
            user.UserName = recordHolderName;

            track = new Track();
            track.BestLapTime = bestLapTime;
            track.Length = trackLength;
            track.Name = trackName;
            track.Id = trackId;

            car = new Car();
            car.Id = carId;
            car.TrackId = trackId;
            car.ImageName = imageName;
            car.Name = name;
            car.Track = new Track();
            car.BestLapTime = bestLapTime;

            driver = new Driver();
            driver.Id = driverId;
            driver.ApplicationUser = user;
            driver.ApplicationUserId = user.Id;
            driver.Car = car;
            driver.CarId = car.Id;
            driver.ControllerId = controllerId;
            driver.Track = track;
            driver.TrackId = track.Id;

            driverResult = new DriverResult();
            driverResult.Id = driverResultId;
            driverResult.ApplicationUser = user;
            driverResult.ApplicationUserId = user.Id;
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
            raceSession.TrackId = trackId;

            laptime = new LapTime();
            laptime.Id = "1";
            laptime.DriverResultId = driverResultId;
            laptime.LapNumber = lapNumber;
            laptime.Time = trackRecord;
        }
        
        [TestMethod]
        public void CarDTO_Test()
        {
            // Setup

            // Act
            CarDTO carDTO = Mapper.Map<CarDTO>(car);
            Car testCar = Mapper.Map<Car>(carDTO);

            // Assert
            Assert.AreEqual(car.Id, carDTO.Id);
            Assert.AreEqual(car.ImageName, carDTO.ImageName);
            Assert.AreEqual(car.Name, carDTO.Name);
            Assert.AreEqual(car.BestLapTime.ApplicationUser.UserName, carDTO.RecordHolder);
            Assert.AreEqual(car.BestLapTime.LapTime.Time, carDTO.TrackRecord);

            Assert.AreEqual(car.Id, testCar.Id);
            Assert.AreEqual(car.ImageName, testCar.ImageName);
            Assert.AreEqual(car.Name, testCar.Name);
        }

        [TestMethod]
        public void DriverDTO_Test()
        {
            // Act
            DriverDTO driverDTO = Mapper.Map<DriverDTO>(driver);
            CarDTO carDTO = Mapper.Map<CarDTO>(driver.Car);

            Driver testDriver = Mapper.Map<Driver>(driverDTO);
            Car testCar = Mapper.Map<Car>(driverDTO.SelectedCar);

            //Assert
            Assert.AreEqual(driver.Id, driverDTO.UserId);
            Assert.AreEqual(driver.ControllerId, driverDTO.ControllerId);
            Assert.AreEqual(driver.ApplicationUser.ImageName, driverDTO.ImageName);
            Assert.AreEqual(carDTO.Id, driverDTO.SelectedCar.Id);
            Assert.AreEqual(carDTO.RecordHolder, driverDTO.SelectedCar.RecordHolder);
            Assert.AreEqual(carDTO.ImageName, driverDTO.SelectedCar.ImageName);
            Assert.AreEqual(carDTO.TrackRecord, driverDTO.SelectedCar.TrackRecord);
            Assert.AreEqual(driver.ApplicationUser.UserName, driverDTO.UserName);

            Assert.AreEqual(driver.ControllerId, testDriver.ControllerId);
            Assert.AreEqual(driver.ApplicationUser.ImageName, testDriver.ApplicationUser.ImageName);
            Assert.AreEqual(carDTO.Id, testDriver.Car.Id);
            Assert.AreEqual(carDTO.RecordHolder, testDriver.Car.BestLapTime.ApplicationUser.UserName);
            Assert.AreEqual(carDTO.ImageName, testDriver.Car.ImageName);
            Assert.AreEqual(carDTO.TrackRecord, testDriver.Car.BestLapTime.LapTime.Time);
            Assert.AreEqual(driver.ApplicationUser.UserName, testDriver.ApplicationUser.UserName);
        }

        [TestMethod]
        public void DriverResultDTO_Test()
        {
            // Act
            DriverResultDTO driverResultDTO = Mapper.Map<DriverResultDTO>(driverResult);
            DriverResult testDriverResult = Mapper.Map<DriverResult>(driverResultDTO);

            // Assert
            Assert.AreEqual(driverResult.Id, driverResultDTO.Id);
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

            Assert.AreEqual(driverResult.Id, testDriverResult.Id);
            Assert.AreEqual(driverResult.BestLapTime, testDriverResult.BestLapTime);
            Assert.AreEqual(driverResult.CarId, testDriverResult.CarId);
            Assert.AreEqual(driverResult.ControllerNumber, testDriverResult.ControllerNumber);
            Assert.AreEqual(driverResult.ApplicationUserId, testDriverResult.ApplicationUserId);
            Assert.AreEqual(driverResult.Finished, testDriverResult.Finished);
            Assert.AreEqual(driverResult.Fuel, testDriverResult.Fuel);
            Assert.AreEqual(driverResult.Laps, testDriverResult.Laps);
            Assert.AreEqual(driverResult.Position, testDriverResult.Position);
            Assert.AreEqual(driverResult.SessionId, testDriverResult.SessionId);
            Assert.AreEqual(driverResult.TimeOffPace, testDriverResult.TimeOffPace);
            Assert.AreEqual(driverResult.TotalTime, testDriverResult.TotalTime);
        }

        [TestMethod]
        public void LapTimeDTO_Test()
        {
            LapTimeDTO laptimeDTO = Mapper.Map<LapTimeDTO>(laptime);
            LapTime testLaptime = Mapper.Map<LapTime>(laptimeDTO);

            Assert.AreEqual(laptime.DriverResultId, laptimeDTO.DriverResultId);
            Assert.AreEqual(laptime.LapNumber, laptimeDTO.LapNumber);
            Assert.AreEqual(laptime.Time, laptimeDTO.Time);

            Assert.AreEqual(laptime.DriverResultId, testLaptime.DriverResultId);
            Assert.AreEqual(laptime.LapNumber, testLaptime.LapNumber);
            Assert.AreEqual(laptime.Time, testLaptime.Time);
        }

        [TestMethod]
        public void RaceSessionDTO_Test()
        {
            // Act 
            RaceSessionDTO raceSessionDTO = Mapper.Map<RaceSessionDTO>(raceSession);
            RaceSession testRaceSession = Mapper.Map<RaceSession>(raceSessionDTO);

            //Assert
            Assert.AreEqual(raceSession.EndTime, raceSessionDTO.EndTime);
            Assert.AreEqual(raceSession.Id, raceSessionDTO.Id);
            Assert.AreEqual(raceSession.NumberOfDrivers, raceSessionDTO.NumberOfDrivers);
            Assert.AreEqual(raceSession.RaceTypeId, raceSessionDTO.RaceTypeId);
            Assert.AreEqual(raceSession.StartTime, raceSessionDTO.StartTime);
            Assert.AreEqual(raceSession.TrackId, raceSessionDTO.TrackId);

            Assert.AreEqual(raceSession.EndTime, testRaceSession.EndTime);
            Assert.AreEqual(raceSession.Id, testRaceSession.Id);
            Assert.AreEqual(raceSession.NumberOfDrivers, testRaceSession.NumberOfDrivers);
            Assert.AreEqual(raceSession.RaceTypeId, testRaceSession.RaceTypeId);
            Assert.AreEqual(raceSession.StartTime, testRaceSession.StartTime);
            Assert.AreEqual(raceSession.TrackId, testRaceSession.TrackId);
        }

        [TestMethod]
        public void RaceTypeDTO_Test()
        {
            // Act
            RaceTypeDTO raceTypeDTO = Mapper.Map<RaceTypeDTO>(raceType);
            RaceType testRaceType = Mapper.Map<RaceType>(raceTypeDTO);

            // Assert
            Assert.AreEqual(raceType.Id, raceTypeDTO.Id);
            Assert.AreEqual(raceType.Name, raceTypeDTO.Name);
            Assert.AreEqual(raceType.Rules, raceTypeDTO.Rules);
            Assert.AreEqual(raceType.Symbol, raceTypeDTO.Symbol);

            Assert.AreEqual(raceType.Id, testRaceType.Id);
            Assert.AreEqual(raceType.Name, testRaceType.Name);
            Assert.AreEqual(raceType.Rules, testRaceType.Rules);
            Assert.AreEqual(raceType.Symbol, testRaceType.Symbol);
        }

        [TestMethod]
        public void TrackDTO_Test()
        {
            // Act
            TrackDTO trackDTO = Mapper.Map<TrackDTO>(track);
            Track testTrack = Mapper.Map<Track>(trackDTO);

            // Assert
            Assert.AreEqual(track.Length, trackDTO.Length);
            Assert.AreEqual(track.Name, trackDTO.Name);

            Assert.AreEqual(track.Length, testTrack.Length);
            Assert.AreEqual(track.Name, testTrack.Name);
        }
    }
}
