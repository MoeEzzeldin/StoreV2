USE store;
GO

-- Insert 50 new sample products for 2025
INSERT INTO product (brand, name, type, price, picture_url) VALUES 
    ('FutureTech', 'QuantumBook', 'Laptop', 2499.99, ''),
    ('OmniCorp', 'HoloLens 3', 'AR Glasses', 1999.99, ''),
    ('CyberDyne', 'Guardian Bot', 'Robot Assistant', 7999.99, ''),
    ('Apple', 'iPhone 16 Pro', 'Smartphone', 1499.99, ''),
    ('Samsung', 'Galaxy S25 Ultra', 'Smartphone', 1399.99, ''),
    ('Google', 'Pixel 9 Pro', 'Smartphone', 1099.99, ''),
    ('Sony', 'PlayStation 6', 'Gaming Console', 599.99, ''),
    ('Microsoft', 'Xbox Series Z', 'Gaming Console', 599.99, ''),
    ('NVIDIA', 'GeForce RTX 5090', 'GPU', 2999.99, ''),
    ('AMD', 'Ryzen 9 9950X', 'CPU', 899.99, ''),
    ('Intel', 'Core i9-15900K', 'CPU', 799.99, ''),
    ('Dell', 'XPS 15 (2025)', 'Laptop', 2199.99, ''),
    ('HP', 'Spectre x360 (2025)', 'Laptop', 1899.99, ''),
    ('Lenovo', 'ThinkPad X1 Carbon Gen 13', 'Laptop', 2399.99, ''),
    ('Apple', 'MacBook Air M4', 'Laptop', 1299.99, ''),
    ('LG', 'UltraGear OLED 32', 'Monitor', 1499.99, ''),
    ('Samsung', 'Odyssey Neo G9 (2025)', 'Monitor', 2299.99, ''),
    ('Bose', 'QuietComfort Ultra', 'Headphones', 449.99, ''),
    ('Sony', 'WH-1000XM6', 'Headphones', 429.99, ''),
    ('Apple', 'AirPods Pro 3', 'Earbuds', 279.99, ''),
    ('Canon', 'EOS R6 Mark II', 'Camera', 2799.99, ''),
    ('Nikon', 'Z7 III', 'Camera', 3499.99, ''),
    ('DJI', 'Mavic 4 Pro', 'Drone', 1799.99, ''),
    ('GoPro', 'Hero 12 Black', 'Action Camera', 499.99, ''),
    ('Tesla', 'Cyberquad', 'ATV', 8999.99, ''),
    ('Boston Dynamics', 'Spot Mini', 'Robot', 7499.99, ''),
    ('Oculus', 'Quest 4', 'VR Headset', 499.99, ''),
    ('Valve', 'Index 2', 'VR Headset', 1199.99, ''),
    ('Fitbit', 'Sense 3', 'Smartwatch', 329.99, ''),
    ('Garmin', 'Fenix 8', 'Smartwatch', 799.99, ''),
    ('Apple', 'Apple Watch Series 10', 'Smartwatch', 499.99, ''),
    ('Samsung', 'Galaxy Watch 7', 'Smartwatch', 429.99, ''),
    ('Sonos', 'Arc 2', 'Soundbar', 999.99, ''),
    ('Razer', 'Blade 16 (2025)', 'Laptop', 2699.99, ''),
    ('Corsair', 'K100 RGB Keyboard', 'Keyboard', 249.99, ''),
    ('Logitech', 'MX Master 5', 'Mouse', 119.99, ''),
    ('Elgato', 'Stream Deck Pro', 'Streaming Device', 299.99, ''),
    ('Blue', 'Yeti X Pro', 'Microphone', 199.99, ''),
    ('Philips Hue', 'Gradient Lightstrip 2', 'Smart Lighting', 179.99, ''),
    ('Nanoleaf', 'Lines Squared', 'Smart Lighting', 249.99, ''),
    ('iRobot', 'Roomba j9+', 'Robot Vacuum', 1099.99, ''),
    ('Dyson', 'V15 Detect Absolute', 'Vacuum', 749.99, ''),
    ('Anker', 'Nebula Cosmos Laser 4K', 'Projector', 2199.99, ''),
    ('BenQ', 'HT4550i', 'Projector', 2999.99, ''),
    ('Synology', 'DS925+', 'NAS', 699.99, ''),
    ('Netgear', 'Orbi 970 Series', 'Mesh WiFi', 1499.99, ''),
    ('TP-Link', 'Deco BE95', 'Mesh WiFi', 1199.99, ''),
    ('Framework', 'Laptop 16', 'Laptop', 1699.99, ''),
    ('Humane', 'AI Pin 2', 'Wearable', 799.99, ''),
    ('Brilliant Labs', 'Frame', 'AR Glasses', 349.99, '');
GO

-- Insert sample reviews for the new products
INSERT INTO review (product_id, reviewer, rating, title, comment, date) VALUES 
    (51, 'TechGuru', 5, 'A Glimpse into the Future', 'The QuantumBook is mind-blowingly fast. Worth every penny.', '2025-01-15'),
    (51, 'EarlyAdopter', 4, 'Impressive Performance', 'Almost perfect, but the battery life could be better.', '2025-01-20'),
    (52, 'AR_Fan', 5, 'Game Changer', 'HoloLens 3 changes everything. The level of immersion is unreal.', '2025-02-10'),
    (53, 'MrRobot', 5, 'My New Best Friend', 'The Guardian Bot is incredibly helpful around the house.', '2025-03-05'),
    (54, 'AppleFan', 5, 'Sleek and Powerful', 'As always, Apple delivers a masterpiece. The camera is incredible.', '2025-04-01'),
    (55, 'SamsungFan', 5, 'The Everything Phone', 'This phone has it all. The screen is stunning.', '2025-04-01'),
    (56, 'PhotoPhil', 5, 'Best Camera on a Phone', 'Google''s computational photography is magic.', '2025-04-12'),
    (57, 'GamerX', 5, 'Next Level Gaming', 'The PS6 is a true next-gen experience. The graphics are insane.', '2025-05-15'),
    (58, 'GamerY', 5, 'Wow!', 'Microsoft has outdone themselves. The best Xbox yet.', '2025-05-16'),
    (59, 'PCBuilder', 5, 'Unbelievable Power', 'The RTX 5090 handles 8K gaming like a dream.', '2025-06-01'),
    (60, 'AMD_Rocks', 5, 'A Beast of a CPU', 'The 9950X is a multitasking monster.', '2025-06-05'),
    (61, 'IntelInside', 4, 'Very Fast', 'Great performance, but it runs a bit hot.', '2025-06-07'),
    (62, 'DesignerDan', 5, 'Perfect for Creatives', 'The new XPS 15 is the ultimate laptop for designers.', '2025-02-20'),
    (63, 'StudentLife', 5, 'Versatile and Stylish', 'The Spectre x360 is perfect for school and entertainment.', '2025-02-22'),
    (64, 'BizTraveler', 5, 'The Ultimate Business Laptop', 'Light, powerful, and durable. The perfect travel companion.', '2025-03-10'),
    (65, 'Writer''sChoice', 5, 'Silent and Powerful', 'The M4 chip is a game-changer for the MacBook Air.', '2025-03-15'),
    (66, 'MovieLover', 5, 'Stunning Visuals', 'OLED is the way to go. This monitor is amazing for movies.', '2025-01-25'),
    (67, 'ImmersiveGamer', 5, 'You Have to See It to Believe It', 'The new Odyssey Neo G9 is an immersive masterpiece.', '2025-02-05'),
    (68, 'Audiophile', 5, 'Pure Silence', 'The noise cancellation on the QuietComfort Ultra is unmatched.', '2025-04-10'),
    (69, 'MusicMan', 5, 'Incredible Sound', 'Sony continues to dominate the headphone market.', '2025-04-11'),
    (70, 'OnTheGo', 5, 'Perfectly Seamless', 'The AirPods Pro 3 are a magical experience.', '2025-05-01'),
    (71, 'ProPhotog', 5, 'A Masterpiece', 'Canon has done it again. The R6 Mark II is a phenomenal camera.', '2025-05-20'),
    (72, 'Nikon Shooter', 5, 'Incredible Detail', 'The Z7 III captures breathtaking images.', '2025-05-25'),
    (73, 'DronePilot', 5, 'The Best Gets Better', 'DJI continues to innovate. The Mavic 4 Pro is a joy to fly.', '2025-06-10'),
    (74, 'AdrenalineJunkie', 5, 'Built for Adventure', 'The Hero 12 is the toughest and most powerful GoPro yet.', '2025-06-15'),
    (75, 'OffRoader', 5, 'Electric Fun', 'The Cyberquad is a blast to ride.', '2025-07-01'),
    (76, 'TechieTom', 4, 'A Bit Spendy, But Cool', 'Spot Mini is an amazing piece of technology.', '2025-07-02'),
    (77, 'VR_Explorer', 5, 'The Future is Here', 'The Oculus Quest 4 is a huge leap forward for VR.', '2025-01-30'),
    (78, 'VR_Enthusiast', 5, 'High-End VR', 'The Index 2 is the ultimate VR experience for those who can afford it.', '2025-02-15'),
    (79, 'FitnessFreak', 5, 'The Best Fitness Tracker', 'The Sense 3 has amazing health tracking features.', '2025-03-20'),
    (80, 'HikerLife', 5, 'Built for the Outdoors', 'The Fenix 8 is the ultimate GPS watch for adventurers.', '2025-03-25'),
    (81, 'AppleWatchUser', 5, 'The Best Smartwatch', 'The Series 10 is a significant upgrade.', '2025-04-05'),
    (82, 'AndroidUser', 5, 'Stylish and Functional', 'The Galaxy Watch 7 is the best Wear OS watch.', '2025-04-08'),
    (83, 'HomeTheaterGuy', 5, 'Immersive Audio', 'The Sonos Arc 2 fills the room with incredible sound.', '2025-05-10'),
    (84, 'RazerFanboy', 5, 'Gaming Powerhouse', 'The Blade 16 is the best gaming laptop on the market.', '2025-06-20'),
    (85, 'ClickyKeys', 5, 'Typing Heaven', 'The K100 is the best mechanical keyboard I have ever used.', '2025-01-10'),
    (86, 'ProductivityPro', 5, 'The Perfect Mouse', 'The MX Master 5 is comfortable and packed with features.', '2025-01-12'),
    (87, 'Streamer', 5, 'Must-Have for Streamers', 'The Stream Deck Pro makes streaming so much easier.', '2025-02-01'),
    (88, 'Podcaster', 5, 'Crystal Clear Audio', 'The Yeti X Pro is perfect for professional-sounding podcasts.', '2025-02-02'),
    (89, 'SmartHomeFan', 5, 'Beautiful Lighting', 'The Gradient Lightstrip 2 creates amazing lighting effects.', '2025-03-01'),
    (90, 'DesignLover', 5, 'Art on the Walls', 'Nanoleaf Lines Squared are a unique and beautiful way to light a room.', '2025-03-02'),
    (91, 'CleanFreak', 5, 'Effortless Cleaning', 'The Roomba j9+ is a fantastic robot vacuum.', '2025-04-20'),
    (92, 'PetOwner', 5, 'Amazing Suction', 'The V15 is incredible at picking up pet hair.', '2025-04-22'),
    (93, 'Cinephile', 5, 'Theater in a Box', 'The Nebula Cosmos Laser 4K is an amazing projector.', '2025-05-30'),
    (94, 'ProjectorPro', 5, 'Stunning 4K', 'The BenQ HT4550i delivers a true cinematic experience.', '2025-06-02'),
    (95, 'DataHoarder', 5, 'Safe and Secure', 'The DS925+ is a reliable and easy-to-use NAS.', '2025-01-18'),
    (96, 'WiFiWizard', 5, 'Blazing Fast WiFi', 'The Orbi 970 covers my whole house in fast, reliable WiFi.', '2025-02-25'),
    (97, 'ConnectedHome', 5, 'No More Dead Zones', 'The Deco BE95 provides seamless WiFi coverage.', '2025-02-28'),
    (98, 'RightToRepair', 5, 'A Laptop for the Future', 'The Framework Laptop 16 is a dream come true for tinkerers.', '2025-03-18'),
    (99, 'Minimalist', 4, 'Interesting Concept', 'The AI Pin 2 is a fascinating device, but its not for everyone.', '2025-06-25'),
    (100, 'EarlyAdopter', 5, 'The Future of Eyewear', 'Brilliant Labs Frame is a glimpse into the future of smart glasses.', '2025-06-30');
GO
