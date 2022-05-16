
/*
 * Opret database => userdb manuelt!
 *

DROP DATABASE IF EXISTS userdb;

CREATE DATABASE userdb
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Danish_Denmark.1252'
    LC_CTYPE = 'Danish_Denmark.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;
*/


/*
 * Enable extension: pgcrypto
 */

CREATE EXTENSION IF NOT EXISTS pgcrypto;

/*
 * Create table: public.role
 */

CREATE TABLE IF NOT EXISTS public.role
(
    roleid smallint NOT NULL,
    name character varying(40) COLLATE pg_catalog."default" NOT NULL,
    description character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT role_pkey PRIMARY KEY (roleid)
);

ALTER TABLE IF EXISTS public.role
    OWNER to postgres;

/*
 * Create table: public.user
 */

CREATE TABLE IF NOT EXISTS public."user"
(
    userid serial NOT NULL,
    name character varying(128) COLLATE pg_catalog."default" NOT NULL,
    emailaddress character varying(128) COLLATE pg_catalog."default",
    pswhash character varying(80) COLLATE pg_catalog."default",
    role smallint DEFAULT 0,
    CONSTRAINT user_pkey PRIMARY KEY (userid),
    CONSTRAINT emailaddress UNIQUE (emailaddress),
    CONSTRAINT user_role FOREIGN KEY (role)
        REFERENCES public.role (roleid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

ALTER TABLE IF EXISTS public."user"
    OWNER to postgres;

/*
 * Add procedure: public.create_user(character varying, character varying, integer, character varying)
 */

DROP PROCEDURE IF EXISTS public.create_user(character varying, character varying, integer, character varying);

CREATE OR REPLACE PROCEDURE public.create_user(
	name character varying,
	email character varying,
	userrole integer,
	password character varying)
LANGUAGE 'plpgsql'
AS $BODY$
DECLARE tmp_email TEXT;

BEGIN
	SELECT emailaddress
	INTO tmp_email
	FROM "user"
	WHERE emailaddress = email;

	if found then
		raise notice 'Email % already exists', email;
	else
		INSERT INTO "user" (name, emailaddress, role, pswhash)
			VALUES (name, email, userrole, crypt(password, gen_salt('bf')) );
	end if;
END 
$BODY$;

ALTER PROCEDURE public.create_user(character varying, character varying, integer, character varying)
    OWNER TO postgres;

/*
 * Add function: public.authenticate_user(character varying, character varying)
 */

DROP FUNCTION IF EXISTS public.authenticate_user(character varying, character varying);

CREATE OR REPLACE FUNCTION public.authenticate_user(
	inemail character varying,
	inpassword character varying)
    RETURNS TABLE(id int, name character varying, emailaddress character varying, role smallint) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$

DECLARE authenticated boolean := false;

BEGIN
 
  SELECT (u.pswhash = crypt(inPassword, u.pswhash)) AS pswmatch 
  	INTO authenticated
  	FROM "user" u
  	WHERE u.emailaddress = inEmail;

  IF authenticated THEN
  	RETURN QUERY
		SELECT u.userid as id, u.name, u.emailaddress, u.role 
		FROM "user" u
		WHERE u.emailaddress = inEmail;
  END IF;

END
$BODY$;

ALTER FUNCTION public.authenticate_user(character varying, character varying)
    OWNER TO postgres;

COMMENT ON FUNCTION public.authenticate_user(character varying, character varying)
    IS 'Authenticates a users credentials.';

