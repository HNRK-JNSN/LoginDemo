/*
 * Insert user roles
 */

 INSERT INTO role ("roleid", "name", "description") VALUES
  (0, 'unassigned', 'Used for users without a role'),
  (1, 'volunteer', 'A volunteer user'),
  (2, 'participant', 'A participant user'),
  (99, 'coordinator', 'A coordinator user');

  /*
   * Insert a coordinator user
   */

INSERT INTO "user" ("name", "emailaddress", "pswhash", "role")
  VALUES ('Mary Schmitt', 'masc@myfestival.io', '$2a$06$65JL/y2.AzUpD6np11bQx.B41hKWBCsisACO8IzYgIGh9D6Dsj.gO', 99);
