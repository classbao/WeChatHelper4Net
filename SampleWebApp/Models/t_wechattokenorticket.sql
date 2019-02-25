/*
Navicat MySQL Data Transfer

Source Server         : rm-bp1yc4bmnn6b63o51vo.mysql.xxx.com
Source Server Version : 50720
Source Host           : rm-bp1yc4bmnn6b63o51vo.mysql.xxx.com:3306
Source Database       : db_machineroomcheckwork

Target Server Type    : MYSQL
Target Server Version : 50720
File Encoding         : 65001

Date: 2019-02-25 15:42:44
开源社区：https://github.com/classbao/WeChatHelper4Net
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for t_wechattokenorticket
-- ----------------------------
DROP TABLE IF EXISTS `t_wechattokenorticket`;
CREATE TABLE `t_wechattokenorticket` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `appid` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `access_token` varchar(2000) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `expires_in` int(11) DEFAULT NULL,
  `expires_time` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `errcode` int(11) DEFAULT NULL,
  `errmsg` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `type` varchar(20) COLLATE utf8mb4_unicode_ci NOT NULL,
  `UpdateTime` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '最后修改时间',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ----------------------------
-- Records of t_wechattokenorticket
-- ----------------------------
INSERT INTO `t_wechattokenorticket` VALUES ('1', 'wx9796df387a0e2fe5', '18_7AW1Tmr_FssOA7b3Zox2mpyyKO8vIc0dI_62YDnCIJOVp_IBNAiWPW-wYhHwxi8nSl1F7v-zNp6tTEwL-Vl-KkDmzW78jS_PMTJfpw3DSBy_UE14_TK5r_qD0hDS8caYP2ejrokP94KbNvfYXFJbABATBF', '7200', '2019-02-25 16:59:30', '0', null, 'AccessToken', '2019-02-25 14:59:31');
INSERT INTO `t_wechattokenorticket` VALUES ('2', 'wx9796df387a0e2fe5', 'HoagFKDcsGMVCIY2vOjf9mnTCVvF9LnpFtU99wLva6CKZmEK7TKZYn_VqJsAPboD9miQvHp4LpF-WjQRVsNvHA', '7200', '2019-02-25 16:59:42', '0', null, 'JSApiTicket', '2019-02-25 14:59:42');

-- ----------------------------
-- Select of t_wechattokenorticket
-- ----------------------------
SELECT * FROM `t_wechattokenorticket`;