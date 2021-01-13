-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: 2019 m. Lap 21 d. 10:22
-- Server version: 10.1.38-MariaDB
-- PHP Version: 7.3.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `skelbimaidb`
--

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `blocks`
--

CREATE TABLE `blocks` (
  `id` int(11) NOT NULL,
  `reason` varchar(255) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `fk_user_id` int(11) NOT NULL,
  `fk_admin_id` int(11) NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `category`
--

CREATE TABLE `category` (
  `id` int(5) NOT NULL,
  `name` varchar(20) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `category`
--

INSERT INTO `category` (`id`, `name`) VALUES
(1, 'Phones'),
(2, 'TV'),
(3, 'Laptop'),
(4, 'PC'),
(5, 'Smart watch'),
(6, 'Headphones'),
(7, 'Console'),
(8, 'Drones'),
(9, 'Camera'),
(10, 'Tablets');

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `comments`
--

CREATE TABLE `comments` (
  `id` int(11) NOT NULL,
  `fk_user_id` int(11) NOT NULL,
  `fk_skelbimas_id` int(11) NOT NULL,
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `date` datetime NOT NULL,
  `like_counter` int(11) NOT NULL DEFAULT '0',
  `dislike_counter` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `comments`
--

INSERT INTO `comments` (`id`, `fk_user_id`, `fk_skelbimas_id`, `description`, `date`, `like_counter`, `dislike_counter`) VALUES
(19, 22, 13, 'yess', '2019-11-19 12:39:34', 2, 0),
(28, 27, 13, 'I like that there are dislikes, but no likes :LUL:', '2019-11-06 23:38:40', 0, 1),
(41, 28, 13, 'Dooot', '2019-11-07 22:13:57', 2, 0),
(50, 22, 53, '10 +10 = 21 ok boomer', '2019-11-09 19:14:32', 0, 1),
(52, 27, 53, 'That\'s rasist thing to say, you know? you should be ashamed, you dirty animated girl on the cover having milenial', '2019-11-09 19:27:38', 0, 0),
(58, 24, 13, 'Gavau 5', '2019-11-12 16:11:53', 3, 0),
(60, 24, 63, 'Dviejų spalvų? Kaip suprast?', '2019-11-13 11:19:21', 0, 0),
(61, 22, 63, 'Klausimas, ar tikrai tavo telikas, nes foto tai iš stalkphoto.com ?', '2019-11-13 11:19:46', 0, 0),
(62, 27, 63, 'Tipo, kai išjungtas viena spalva ir kai įjungtas kita spalva', '2019-11-13 11:19:56', 2, 0),
(63, 27, 63, 'Mano ne mano, pats iš sukvežimio pavog... paėmiau', '2019-11-19 13:10:07', 0, 0),
(64, 22, 58, 'Kur gyveni?', '2019-11-13 12:08:36', 0, 0),
(65, 22, 63, 'Tipo is sukciu vezimo (sukvežimio) pavog... paemete?', '2019-11-13 16:31:55', 0, 0),
(67, 27, 65, 'Kažkoks prastas, nerekomenduoju', '2019-11-14 13:59:40', 0, 0);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `commentsrating`
--

CREATE TABLE `commentsrating` (
  `id` int(11) NOT NULL,
  `fk_user_id` int(11) NOT NULL,
  `fk_comment_id` int(11) NOT NULL,
  `upvote` int(1) NOT NULL DEFAULT '0',
  `downvote` int(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `commentsrating`
--

INSERT INTO `commentsrating` (`id`, `fk_user_id`, `fk_comment_id`, `upvote`, `downvote`) VALUES
(686, 28, 41, 1, 0),
(921, 22, 19, 1, 0),
(925, 27, 50, 0, 1),
(926, 22, 41, 1, 0),
(928, 27, 19, 1, 0),
(929, 27, 28, 0, 1),
(930, 22, 58, 1, 0),
(931, 24, 58, 1, 0),
(932, 27, 58, 1, 0),
(933, 22, 62, 1, 0),
(934, 24, 62, 1, 0);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `country`
--

CREATE TABLE `country` (
  `id` int(11) NOT NULL,
  `name` varchar(30) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `country`
--

INSERT INTO `country` (`id`, `name`) VALUES
(1, 'Vilnius'),
(2, 'Kaunas'),
(3, 'Klaipėda'),
(4, 'Šiauliai'),
(5, 'Panevėžys'),
(6, 'Alytus'),
(7, 'Marijampolė'),
(8, 'Mažeikiai'),
(9, 'Jonava'),
(10, 'Utena'),
(11, 'Kėdainiai'),
(12, 'Telšiai'),
(13, 'Tauragė'),
(14, 'Ukmergė'),
(15, 'Visaginas'),
(16, 'Plungė'),
(17, 'Kretinga'),
(18, 'Gargždai'),
(19, 'Šilutė'),
(20, 'Radviliškis'),
(21, 'Palanga'),
(22, 'Biržai'),
(23, 'Rokiškis'),
(24, 'Kuršėnai'),
(25, 'Druskininkai'),
(26, 'Elektrėnai'),
(27, 'Garliava'),
(28, 'Jurbarkas'),
(29, 'Raseiniai'),
(30, 'Vilkaviškis'),
(31, 'Lentvaris'),
(32, 'Joniškis'),
(33, 'Grigiškės'),
(34, 'Anykščiai'),
(35, 'Kelmė'),
(36, 'Varėna'),
(37, 'Prienai'),
(38, 'Kaišiadorys'),
(39, 'Naujoji Akmenė'),
(40, 'Pasvalys'),
(41, 'Kupiškis'),
(42, 'Zarasai'),
(43, 'Skuodas'),
(44, 'Kazlų Rūda'),
(45, 'Širvintos'),
(46, 'Molėtai'),
(47, 'Šalčininkai'),
(48, 'Šakiai'),
(49, 'Švenčionėliai'),
(50, 'Pabradė'),
(51, 'Kybartai'),
(52, 'Ignalina'),
(53, 'Šilalė'),
(54, 'Nemenčinė'),
(55, 'Pakruojis'),
(56, 'Švenčionys'),
(57, 'Trakai'),
(58, 'Vievis'),
(59, 'Kalvarija'),
(60, 'Lazdijai'),
(61, 'Rietavas'),
(62, 'Žiežmariai'),
(63, 'Eišiškės'),
(64, 'Ariogala'),
(65, 'Neringa'),
(66, 'Šeduva'),
(67, 'Birštonas'),
(68, 'Venta'),
(69, 'Akmenė'),
(70, 'Tytuvėnai'),
(71, 'Rūdiškės'),
(72, 'Vilkija'),
(73, 'Pagėgiai'),
(74, 'Viekšniai'),
(75, 'Žagarė'),
(76, 'Ežerėlis'),
(77, 'Skaudvilė'),
(78, 'Gelgaudiškis'),
(79, 'Kudirkos Naumiestis'),
(80, 'Simnas'),
(81, 'Salantai'),
(82, 'Linkuva'),
(83, 'Ramygala'),
(84, 'Priekulė'),
(85, 'Veisiejai'),
(86, 'Daugai'),
(87, 'Joniškėlis'),
(88, 'Jieznas'),
(89, 'Obeliai'),
(90, 'Varniai'),
(91, 'Virbalis'),
(92, 'Seda'),
(93, 'Vabalninkas'),
(94, 'Subačius'),
(95, 'Baltoji Vokė'),
(96, 'Pandėlys'),
(97, 'Dūkštas'),
(98, 'Užventis'),
(99, 'Dusetos'),
(100, 'Kavarskas'),
(101, 'Smalininkai'),
(102, 'Troškūnai'),
(103, 'Panemunė');

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `forumcategory`
--

CREATE TABLE `forumcategory` (
  `id` int(11) NOT NULL,
  `name` varchar(60) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `view_counter` int(11) NOT NULL DEFAULT '0',
  `fk_user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `forumcategory`
--

INSERT INTO `forumcategory` (`id`, `name`, `description`, `view_counter`, `fk_user_id`) VALUES
(1, 'testerino', 'Sample desc here', 95, 22),
(2, 'aaaa', 'plx work', 2, 22),
(3, 'Aleliuja', 'Vaikai mano', 9, 22),
(4, 'my at', 'is a c', 1, 27);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `forumcomments`
--

CREATE TABLE `forumcomments` (
  `id` int(11) NOT NULL,
  `fk_user_id` int(11) NOT NULL,
  `fk_topic_id` int(11) NOT NULL,
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `like_counter` int(11) NOT NULL DEFAULT '0',
  `dislike_counter` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `forumcomments`
--

INSERT INTO `forumcomments` (`id`, `fk_user_id`, `fk_topic_id`, `description`, `date`, `like_counter`, `dislike_counter`) VALUES
(3, 22, 1, 'Mah tird cumment UwU', '2019-11-08 16:14:58', 0, 1),
(4, 22, 13, 'Ka mesti vartoti?', '2019-11-08 18:51:38', 0, 0),
(7, 22, 14, 'Just crash :4head:', '2019-11-08 22:18:18', 2, 0),
(8, 28, 8, 'ąčęėįšųū90-žqwertyuiop[]]\\asdfghjkl;\'zxc', '2019-11-08 22:33:29', 0, 0),
(9, 28, 13, 'ß', '2019-11-08 22:47:09', 0, 0),
(10, 28, 13, '', '2019-11-08 22:48:29', 0, 0),
(11, 28, 13, 'cos⁡α+cos⁡β=2 cos⁡〖1/2 (α+β)〗  cos⁡〖1/2 (α-β)〗', '2019-11-08 22:49:01', 0, 0),
(13, 27, 14, 'apvaisinau ir apariau', '2019-11-09 20:07:28', 0, 1),
(14, 22, 9, 'maybe because it\'s a GOD DAMN website that has only Electronics hence the name???', '2019-11-09 22:51:21', 0, 0),
(15, 27, 5, 'I\'m serious', '2019-11-12 14:15:42', 1, 0),
(16, 27, 1, 'mah 4rd communt OWO', '2019-11-12 14:29:54', 0, 1),
(18, 32, 1, 'UwO', '2019-11-19 13:21:35', 0, 0);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `forumcommentsrating`
--

CREATE TABLE `forumcommentsrating` (
  `id` int(11) NOT NULL,
  `fk_user_id` int(11) NOT NULL,
  `fk_forum_comment_id` int(11) NOT NULL,
  `upvote` int(1) NOT NULL DEFAULT '0',
  `downvote` int(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `forumcommentsrating`
--

INSERT INTO `forumcommentsrating` (`id`, `fk_user_id`, `fk_forum_comment_id`, `upvote`, `downvote`) VALUES
(17, 28, 7, 1, 0),
(20, 22, 3, 0, 1),
(21, 22, 13, 0, 1),
(22, 22, 7, 1, 0),
(23, 22, 16, 0, 1),
(24, 22, 15, 1, 0);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `messagereceiver`
--

CREATE TABLE `messagereceiver` (
  `id` int(11) NOT NULL,
  `sender_id` int(11) NOT NULL,
  `receiver_id` int(11) NOT NULL,
  `message_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `messagereceiver`
--

INSERT INTO `messagereceiver` (`id`, `sender_id`, `receiver_id`, `message_id`) VALUES
(3, 22, 24, 5),
(7, 22, 32, 8),
(8, 22, 32, 9),
(9, 22, 32, 10),
(10, 22, 24, 11),
(11, 22, 27, 12),
(12, 24, 22, 13),
(13, 22, 24, 14),
(15, 24, 22, 16),
(16, 22, 24, 17),
(20, 22, 22, 21),
(21, 22, 22, 22),
(22, 22, 22, 23),
(23, 22, 24, 24);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `messages`
--

CREATE TABLE `messages` (
  `id` int(11) NOT NULL,
  `message` varchar(255) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `messages`
--

INSERT INTO `messages` (`id`, `message`, `date`) VALUES
(3, 'Balvonas', '2019-11-19 14:28:47'),
(4, 'Balvonas', '2019-11-19 14:30:00'),
(5, 'Balvonas', '2019-11-19 14:31:50'),
(6, 'Balvonas', '2019-11-19 15:26:59'),
(7, 'Kitas balvonas', '2019-11-19 19:57:09'),
(8, 'ka tu mergit', '2019-11-19 20:13:51'),
(9, 'Kaaa', '2019-11-19 20:17:44'),
(10, 'aaaaaaa', '2019-11-19 20:18:46'),
(11, 'Tikram balvonui tikras message', '2019-11-19 20:21:24'),
(12, 'Nelegalus message', '2019-11-19 20:21:42'),
(13, 'Pafol nakui!', '2019-11-19 20:22:51'),
(14, 'Svepliau, issitrauk pirsta is burnos &#128520', '2019-11-19 20:25:12'),
(15, '&#128520', '2019-11-19 20:28:38'),
(16, 'Gelai diede.', '2019-11-19 20:29:19'),
(17, 'Imk pavalgyt ?', '2019-11-19 20:32:20'),
(18, '&#x1F354', '2019-11-19 20:33:33'),
(19, 'ąčėįčąįšųųęėįų', '2019-11-19 20:36:44'),
(20, 'test', '2019-11-19 21:06:55'),
(21, 'asdad', '2019-11-19 21:16:24'),
(22, 'anatha', '2019-11-19 21:16:32'),
(23, 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', '2019-11-20 11:30:22'),
(24, 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', '2019-11-20 11:30:48');

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `messagesender`
--

CREATE TABLE `messagesender` (
  `id` int(11) NOT NULL,
  `sender_id` int(11) NOT NULL,
  `receiver_id` int(11) NOT NULL,
  `message_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `messagesender`
--

INSERT INTO `messagesender` (`id`, `sender_id`, `receiver_id`, `message_id`) VALUES
(9, 22, 24, 11),
(10, 22, 27, 12),
(11, 24, 22, 13),
(12, 22, 24, 14),
(14, 24, 22, 16),
(15, 22, 24, 17),
(18, 22, 22, 20),
(20, 22, 22, 22),
(21, 22, 22, 23),
(22, 22, 24, 24);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `roles`
--

CREATE TABLE `roles` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `roles`
--

INSERT INTO `roles` (`id`, `name`) VALUES
(1, 'user'),
(2, 'admin');

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `skelbimas`
--

CREATE TABLE `skelbimas` (
  `id` int(11) NOT NULL,
  `fk_user_id` int(11) NOT NULL,
  `fk_category_id` int(11) NOT NULL,
  `picture` varchar(255) DEFAULT NULL,
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `title` varchar(30) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `price` decimal(8,2) NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `view_counter` int(10) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `skelbimas`
--

INSERT INTO `skelbimas` (`id`, `fk_user_id`, `fk_category_id`, `picture`, `description`, `title`, `price`, `date`, `view_counter`) VALUES
(13, 22, 2, '/ads/13/index/indexPicture.jpg', 'Sample.pptx', 'Dar vienas skelbimas (updated)', '420.69', '2019-11-09 21:11:13', 0),
(48, 27, 8, '/ads/48/index/indexPicture.png', 'If you know, then please, answer me, I\'ll pay', 'Is this loss?', '50.01', '2019-11-12 14:06:56', 0),
(49, 27, 2, '/ads/49/index/indexPicture.jpg', 'hmm', 'Cabagge', '50.02', '2019-11-09 19:02:33', 0),
(53, 27, 1, '/ads/53/index/indexPicture.jpg', 'two cats, healthy, 2 and 3 years old, very nice behaviour, 10 bucks each', 'pepega', '21.00', '2019-11-09 19:12:05', 0),
(55, 27, 1, '/ads/55/index/indexPicture.jpg', 'this work?', 'jaz', '420.69', '2019-11-09 19:16:51', 0),
(56, 27, 2, NULL, 'I sell feels. 500 each', 'Feels', '500.00', '2019-11-09 19:19:01', 0),
(57, 27, 9, '/ads/57/index/indexPicture.jpg', 'cmon work', '50', '50.00', '2019-11-09 19:21:30', 0),
(58, 24, 8, '/ads/58/index/indexPicture.jpg', '2.0 litrai benzas, su turbo. Gerai ratus suka, kai gazą paspaudi.\r\nPrieš mėnesį pakeisti stabdžių diskai.\r\nKaina derinama.', 'Subaru Impreza WRX', '5500.00', '2019-11-13 11:15:56', 0),
(59, 27, 4, '/ads/59/index/indexPicture.jpg', 'I\'m desperate...', 'Looking for Tarkovsky\'s Mirror', '999.12', '2019-11-09 19:48:31', 0),
(61, 27, 8, '/ads/61/index/indexPicture.jpg', 'caturday gift cards are accepted', 'long cat', '99.43', '2019-11-12 14:10:03', 0),
(62, 27, 2, '/ads/62/index/indexPicture.jpg', 'Beveik nenaudotas, tikrai geras, parduodu nes veikia ir senesnis', 'Visiskai naujas televizorius', '500.61', '2019-11-13 11:14:57', 0),
(63, 27, 2, '/ads/63/index/indexPicture.jpg', 'Nuostabus, penkiasdesimt metu veike be gedimu.', 'Televizorius, dvieju spalvu', '741.11', '2019-11-13 11:16:23', 0),
(64, 27, 5, '/ads/64/index/indexPicture.jpg', 'puikiai ismano laika, todel ir ismanus. kainos nederinu', 'laikrodis, ismanus', '199.98', '2019-11-13 11:18:33', 0),
(65, 31, 3, '/ads/65/index/indexPicture.jpg', 'asd', 'qwe', '50.00', '2019-11-13 11:34:21', 0),
(66, 32, 1, NULL, 'ne', 'taip', '12.00', '2019-11-20 12:00:47', 11);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `skelbimasrating`
--

CREATE TABLE `skelbimasrating` (
  `id` int(11) NOT NULL,
  `fk_user_id` int(11) NOT NULL,
  `fk_skelbimas_id` int(11) NOT NULL,
  `rating` int(11) NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `skelbimasrating`
--

INSERT INTO `skelbimasrating` (`id`, `fk_user_id`, `fk_skelbimas_id`, `rating`, `date`) VALUES
(2, 22, 66, 5, '2019-11-21 11:17:48');

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `topic`
--

CREATE TABLE `topic` (
  `id` int(11) NOT NULL,
  `title` varchar(60) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `description` varchar(255) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `view_counter` int(11) NOT NULL DEFAULT '0',
  `fk_user_id` int(11) NOT NULL,
  `fk_forumcategory_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `topic`
--

INSERT INTO `topic` (`id`, `title`, `description`, `date`, `view_counter`, `fk_user_id`, `fk_forumcategory_id`) VALUES
(1, 'TEstEWW', 'Mano gyvenimo istorija. Pagaiga.', '2019-11-06 21:46:57', 50, 22, 1),
(5, 'Please, ban John Doe', 'That Oedipus wannabe keeps spamming comments on every post', '2019-11-06 23:02:48', 3, 27, 1),
(7, 'some cool meme here', 'https://www.youtube.com/watch?v=dQw4w9WgXcQ', '2019-11-06 23:05:31', 0, 27, 1),
(8, 'MoTes?', '????‍♂️?‍♂️ąčšūž', '2019-11-06 23:07:27', 0, 28, 1),
(9, 'why there isn\'t category for cabagges?', 'I want to buy a purple one, why there are no category for that?', '2019-11-06 23:11:37', 2, 27, 1),
(10, 'Skelbimu nuotraukos', 'Parekomendavo man sita skelbimu puslapi draugas, sake geras. Galvojau reik uzsukt gal kazka rasiu nusipirkt, taciau kur tik paziuresi ten vien tie patys zali batai. Kame cia bazarai, A?!', '2019-11-06 23:36:16', 0, 22, 1),
(12, 'Dalius bez Dalius', 'fuuu,Daliau! Kaip tau negėda?', '2019-11-07 22:22:35', 0, 24, 1),
(13, 'Boba miega, o aš negaliu', 'Ar man jau laikas mesti vartoti, jei pyragas miega, o aš ne?', '2019-11-07 22:57:41', 8, 27, 3),
(14, 'MK11', 'Teadasdd', '2019-11-08 22:17:25', 8, 28, 1),
(15, 'brangūs steroidai', 'Vienas vartototototototototototototototototojas pardavinėja brangius steroidus, tai įdomu, ar geri?', '2019-11-09 20:08:57', 2, 27, 1),
(16, 'diskusija', 'cia diskusija', '2019-11-20 11:15:25', 0, 32, 1);

-- --------------------------------------------------------

--
-- Sukurta duomenų struktūra lentelei `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `username` varchar(20) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `email` varchar(30) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci NOT NULL,
  `fk_country_id` int(11) NOT NULL,
  `role` int(2) NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `profile_picture` varchar(255) DEFAULT NULL,
  `first_name` varchar(20) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci DEFAULT NULL,
  `last_name` varchar(30) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci DEFAULT NULL,
  `phone_number` varchar(11) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci DEFAULT NULL,
  `address` varchar(40) CHARACTER SET utf8 COLLATE utf8_lithuanian_ci DEFAULT NULL,
  `hash` binary(64) NOT NULL,
  `salt` binary(128) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Sukurta duomenų kopija lentelei `user`
--

INSERT INTO `user` (`id`, `username`, `email`, `fk_country_id`, `role`, `date`, `profile_picture`, `first_name`, `last_name`, `phone_number`, `address`, `hash`, `salt`) VALUES
(22, 'CheerLeader', 'dalius.luksa1@gmail.com', 8, 2, '2019-11-12 15:05:39', '/users/22/ahegao.gif', 'Dalius', NULL, '867821352', 'karoliu g. 17', 0x0967831ce09d173a26a49c2ec1e23f552caa7152db695a0648dc522d543382240e4b8c506797c52aec515c0896e949872f8d0e5f6fcf6c4d36690f5d2c0b7449, 0x3b49c7309172f01c98b780598d7460becec0245171798e0da8dbecdb5c0d94e60e9529ed30eba779c01bfd40d0141a4437d12dcf44bf3be250a715308ea28215ccea923cb55f3404ed6c6c937350260b318d80dc9d0fd03bcd5718c7aa77775514c699b80a9afb1225e2da1b4ab008078bc1307d7e3692798eef444cf9a72c7a),
(24, 'karmalsunumeriu3', 'karolis.maliauskas@gmail.com', 11, 1, '2019-11-12 15:05:39', '/users/24/confusedwat.png', 'Karole', 'Maliauskaite', '864512369', 'chem 8-15', 0x9bb9ef6e2a4a334814e1b9123fca597c3257049871d57b454987fa87d6bdf731a3310251701418e412d1bf297744ff44843bf2134efac17782a2e451279c5088, 0x168e4f4918dec1e08b6c56e9ddbf8bd6c21fbef2b9426485debdd586f0891b49812842c4e7165563d868822b0bb47dc8007b2bdcc3299e27956f8313b5a60f1711539be77775f24dbeb8987d4632d5a6a7e204fc3e610f8880d6595dd75e47e1da0c8f30733b49ffc257f449ee4f4abacf8b8e0f1835ccdc29fd07d39bf5b613),
(27, 'LegalusPardavejas', 'kaspa181@gmail.com', 12, 2, '2019-11-12 15:05:39', '/users/27/uncle iroh.gif', 'Kasparas', 'NeTavoReikalas', '100111010', 'Nevarenu gatve 16', 0x2f2fe32cfd9178ac46dce824560221ae25afcae0aa339af73faafd19330454085abc56108b7ac8c73d74a7c4b741942836167ffddcfb56aa1132f4476f80e89a, 0xbc8bd7f4eb52ffb6ada2e9bdab88c98efea8dfca508c6a53376285bdefcb418dd58ed016bad48f4f02e8216fb785ddf374d4b50cf9bcdd249395d33d0088b2564e470a9343b0729f72be44b34b4630fecde0e63d85b15ff4e8b06a813680fadd31b67558db4272f68de87cc375f6bc8a7ae59cfef3c57cc706240a4ddd0d79a1),
(28, 'ProjectDestroyer', 'pepega@pepega.pepega', 6, 1, '2019-11-12 15:05:39', '/users/28/image-missing.png', 'Dot', 'Destroyer', '869853211', 'cekijos g. 2c', 0xea3882e5a52a5b68aab464e0d136379289f4200a7e87606ec45a5d5d3584135e478fa5759635d737ca4db69e5865f93957ef1932a14872381d832aa448e27464, 0x1be0efc0a0515211f8d9bc594f1dac31f25b6713d8c6821099f95ce1a7aebe406a5b19fde8d6366b265233293c75c9df001dd94bb870796372a1f218ded38e74273db8d90d5d1799b863375c4d04735237b54bb0c3b32047b85899e3f8102c97e5856f20bd64a118f5d03d0d1d0d362bdb12eb2450b177788cb064d51b154905),
(31, 'asdasd', 'asd@asd.asd', 16, 1, '2019-11-13 11:33:10', NULL, NULL, NULL, NULL, NULL, 0xf68790565f7f4ae6e825bafec48d0d4d1dc65241022e26d765b82ee989075d4cc2016c63f86623e41962a18d1120ac5048cff6776c79582d7485b69c20527c80, 0x9e7b438b2d13352470f5c698e9cfa3746fd53892b2bab201bb4cdad49aed3268b07e8866c27424b54047ea273f42b7b2e02fb60adc3e7dd02fdd621f744dd79cfe0ba3f746a20ca47708b57ae738da20afe5700355525c33e94dbc1db5e37b2c4a1dffd70561dffdb3dcb5bb80dec7b13d215269e1f8a5b2d8b695c26ec4764e),
(32, 'testukas', 'grill@grill.grill', 10, 1, '2019-11-19 13:08:15', NULL, NULL, NULL, NULL, NULL, 0xa46c18951df0df0c6c343cbb9afcf7749a239a328a7ce5077568e136d61923c8e757f4b342b90bf7b6c998d4899735b01d34e23e8f294efdc957bc02ea2d8529, 0xc65ad746676fcf25f4b4acd21a9129a2a35793d7223167f7e99c7c024b9ee10f7dfca4d69f118a4e377c8461f9c89881155f41cd904344bad0638fd308380595131a961f80181d296e30d655fbe76b7f5968038fe46ca57e11fe22aab9144d20568b653e0bc70706f523976a24095670acc5dd448b66401821ab348772308b37),
(33, 'Weeeeb', 'weeb@a', 5, 1, '2019-11-19 20:36:38', NULL, NULL, NULL, NULL, NULL, 0x289a7e4bd5b8d68cf5f8dfb9507dc74cd9d451a473a2d02084f54ed8a21f7b6c31ae6383e938ff144fdf3c71683f1e7593be94f5a8c61c85252fdc9e98ec7942, 0xafc3526325f89acff1245567cbc373c15541c521142a5f8ef11ee6acbf343affd93ef646803d9b8998ec044381bac27a92f8437ec104ede77c2ce0834f21e89fa39a3f8a9f27c904cc9b740265682180f3d630561cf1013d7e97c7ccd2452dd5666c284f5a4229c4684dad525dd3df860d6e59bd68301b35372fc9fe27ab6f67);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `blocks`
--
ALTER TABLE `blocks`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `fk_user_id` (`fk_user_id`),
  ADD KEY `fk_admin_id` (`fk_admin_id`);

--
-- Indexes for table `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `comments`
--
ALTER TABLE `comments`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`fk_user_id`),
  ADD KEY `skelbimas_id` (`fk_skelbimas_id`);

--
-- Indexes for table `commentsrating`
--
ALTER TABLE `commentsrating`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_user_id` (`fk_user_id`),
  ADD KEY `fk_comment_id` (`fk_comment_id`);

--
-- Indexes for table `country`
--
ALTER TABLE `country`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `forumcategory`
--
ALTER TABLE `forumcategory`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_user_id` (`fk_user_id`);

--
-- Indexes for table `forumcomments`
--
ALTER TABLE `forumcomments`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_user_id` (`fk_user_id`),
  ADD KEY `fk_topic_id` (`fk_topic_id`);

--
-- Indexes for table `forumcommentsrating`
--
ALTER TABLE `forumcommentsrating`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_user_id` (`fk_user_id`),
  ADD KEY `fk_forum_comment_id` (`fk_forum_comment_id`);

--
-- Indexes for table `messagereceiver`
--
ALTER TABLE `messagereceiver`
  ADD PRIMARY KEY (`id`),
  ADD KEY `sender_id` (`sender_id`),
  ADD KEY `receiver_id` (`receiver_id`),
  ADD KEY `message_id` (`message_id`);

--
-- Indexes for table `messages`
--
ALTER TABLE `messages`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `messagesender`
--
ALTER TABLE `messagesender`
  ADD PRIMARY KEY (`id`),
  ADD KEY `sender_id` (`sender_id`),
  ADD KEY `receiver_id` (`receiver_id`),
  ADD KEY `message_id` (`message_id`);

--
-- Indexes for table `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `skelbimas`
--
ALTER TABLE `skelbimas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`fk_user_id`),
  ADD KEY `category_id` (`fk_category_id`);

--
-- Indexes for table `skelbimasrating`
--
ALTER TABLE `skelbimasrating`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_user_id` (`fk_user_id`),
  ADD KEY `fk_skelbimas_id` (`fk_skelbimas_id`);

--
-- Indexes for table `topic`
--
ALTER TABLE `topic`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_user_id` (`fk_user_id`),
  ADD KEY `fk_forumcategory_id` (`fk_forumcategory_id`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `Unique username` (`username`),
  ADD UNIQUE KEY `Unique email` (`email`),
  ADD KEY `country_id` (`fk_country_id`) USING BTREE,
  ADD KEY `role` (`role`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `blocks`
--
ALTER TABLE `blocks`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `category`
--
ALTER TABLE `category`
  MODIFY `id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `comments`
--
ALTER TABLE `comments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=68;

--
-- AUTO_INCREMENT for table `commentsrating`
--
ALTER TABLE `commentsrating`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=935;

--
-- AUTO_INCREMENT for table `country`
--
ALTER TABLE `country`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=104;

--
-- AUTO_INCREMENT for table `forumcategory`
--
ALTER TABLE `forumcategory`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `forumcomments`
--
ALTER TABLE `forumcomments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT for table `forumcommentsrating`
--
ALTER TABLE `forumcommentsrating`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `messagereceiver`
--
ALTER TABLE `messagereceiver`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT for table `messages`
--
ALTER TABLE `messages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `messagesender`
--
ALTER TABLE `messagesender`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `roles`
--
ALTER TABLE `roles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `skelbimas`
--
ALTER TABLE `skelbimas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=67;

--
-- AUTO_INCREMENT for table `skelbimasrating`
--
ALTER TABLE `skelbimasrating`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `topic`
--
ALTER TABLE `topic`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=34;

--
-- Apribojimai eksportuotom lentelėm
--

--
-- Apribojimai lentelei `blocks`
--
ALTER TABLE `blocks`
  ADD CONSTRAINT `blocks_ibfk_1` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `blocks_ibfk_2` FOREIGN KEY (`fk_admin_id`) REFERENCES `user` (`id`);

--
-- Apribojimai lentelei `comments`
--
ALTER TABLE `comments`
  ADD CONSTRAINT `comments_ibfk_1` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `comments_ibfk_2` FOREIGN KEY (`fk_skelbimas_id`) REFERENCES `skelbimas` (`id`);

--
-- Apribojimai lentelei `commentsrating`
--
ALTER TABLE `commentsrating`
  ADD CONSTRAINT `commentsrating_ibfk_1` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `commentsrating_ibfk_2` FOREIGN KEY (`fk_comment_id`) REFERENCES `comments` (`id`);

--
-- Apribojimai lentelei `forumcategory`
--
ALTER TABLE `forumcategory`
  ADD CONSTRAINT `forumcategory_ibfk_1` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`);

--
-- Apribojimai lentelei `forumcomments`
--
ALTER TABLE `forumcomments`
  ADD CONSTRAINT `forumcomments_ibfk_1` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `forumcomments_ibfk_2` FOREIGN KEY (`fk_topic_id`) REFERENCES `topic` (`id`);

--
-- Apribojimai lentelei `forumcommentsrating`
--
ALTER TABLE `forumcommentsrating`
  ADD CONSTRAINT `forumcommentsrating_ibfk_1` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `forumcommentsrating_ibfk_2` FOREIGN KEY (`fk_forum_comment_id`) REFERENCES `forumcomments` (`id`);

--
-- Apribojimai lentelei `messagereceiver`
--
ALTER TABLE `messagereceiver`
  ADD CONSTRAINT `messagereceiver_ibfk_1` FOREIGN KEY (`sender_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `messagereceiver_ibfk_2` FOREIGN KEY (`receiver_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `messagereceiver_ibfk_3` FOREIGN KEY (`message_id`) REFERENCES `messages` (`id`);

--
-- Apribojimai lentelei `messagesender`
--
ALTER TABLE `messagesender`
  ADD CONSTRAINT `messagesender_ibfk_1` FOREIGN KEY (`sender_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `messagesender_ibfk_2` FOREIGN KEY (`receiver_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `messagesender_ibfk_3` FOREIGN KEY (`message_id`) REFERENCES `messages` (`id`);

--
-- Apribojimai lentelei `skelbimas`
--
ALTER TABLE `skelbimas`
  ADD CONSTRAINT `skelbimas_ibfk_1` FOREIGN KEY (`fk_category_id`) REFERENCES `category` (`id`),
  ADD CONSTRAINT `skelbimas_ibfk_2` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`);

--
-- Apribojimai lentelei `skelbimasrating`
--
ALTER TABLE `skelbimasrating`
  ADD CONSTRAINT `skelbimasrating_ibfk_1` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `skelbimasrating_ibfk_2` FOREIGN KEY (`fk_skelbimas_id`) REFERENCES `skelbimas` (`id`);

--
-- Apribojimai lentelei `topic`
--
ALTER TABLE `topic`
  ADD CONSTRAINT `topic_ibfk_1` FOREIGN KEY (`fk_user_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `topic_ibfk_2` FOREIGN KEY (`fk_forumcategory_id`) REFERENCES `forumcategory` (`id`);

--
-- Apribojimai lentelei `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `user_ibfk_1` FOREIGN KEY (`fk_country_id`) REFERENCES `country` (`id`),
  ADD CONSTRAINT `user_ibfk_2` FOREIGN KEY (`role`) REFERENCES `roles` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
