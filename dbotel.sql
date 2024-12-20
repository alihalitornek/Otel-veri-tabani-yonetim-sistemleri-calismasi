--
-- PostgreSQL database dump
--

-- Dumped from database version 16.4
-- Dumped by pg_dump version 16.4

-- Started on 2024-12-20 14:15:14

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 268 (class 1255 OID 25208)
-- Name: bakim_talebi_durum_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.bakim_talebi_durum_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.durum NOT IN ('beklemede', 'işlemde', 'tamamlandı') THEN
        RAISE EXCEPTION 'Durum alanı sadece beklemede, işlemde veya tamamlandı değerlerinden birini alabilir';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.bakim_talebi_durum_kontrol() OWNER TO postgres;

--
-- TOC entry 267 (class 1255 OID 25206)
-- Name: bakim_talep_tarihi_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.bakim_talep_tarihi_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.talep_tarihi > CURRENT_DATE THEN
        RAISE EXCEPTION 'talep_tarihi gelecekte bir tarih olamaz. Güncel veya geçmiş bir tarih giriniz';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.bakim_talep_tarihi_kontrol() OWNER TO postgres;

--
-- TOC entry 271 (class 1255 OID 25214)
-- Name: etkinlik_kapasite_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.etkinlik_kapasite_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.kapasite <= 0 THEN
        RAISE EXCEPTION 'Kapasite alanı 0 dan büyük olmalıdır';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.etkinlik_kapasite_kontrol() OWNER TO postgres;

--
-- TOC entry 262 (class 1255 OID 25194)
-- Name: fatura_toplam_ucret_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.fatura_toplam_ucret_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.toplam_ücret <= 0 THEN
        RAISE EXCEPTION 'toplam_ücret alanı 0 dan büyük olmalıdır';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.fatura_toplam_ucret_kontrol() OWNER TO postgres;

--
-- TOC entry 263 (class 1255 OID 25197)
-- Name: fatura_ödeme_yöntemi_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."fatura_ödeme_yöntemi_kontrol"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.ödeme_yöntemi NOT IN ('nakit', 'kredi kartı', 'havale', 'online') THEN
        RAISE EXCEPTION 'ödeme_yöntemi sadece nakit, kredi kartı, havale veya online olabilir';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public."fatura_ödeme_yöntemi_kontrol"() OWNER TO postgres;

--
-- TOC entry 272 (class 1255 OID 25216)
-- Name: geri_bildirim_puan_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.geri_bildirim_puan_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.puan < 0 OR NEW.puan > 10 THEN
        RAISE EXCEPTION 'Puan 0 ile 10 arasında bir değer olmalıdır';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.geri_bildirim_puan_kontrol() OWNER TO postgres;

--
-- TOC entry 286 (class 1255 OID 25228)
-- Name: hizmet_ucret_guncelle(integer, real); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.hizmet_ucret_guncelle(IN hizmet_id integer, IN yeni_ucret real)
    LANGUAGE plpgsql
    AS $$
DECLARE
    eski_ucret REAL;
BEGIN
    SELECT ücret INTO eski_ucret FROM hizmet WHERE hizmet.hizmet_id = hizmet_ucret_guncelle.hizmet_id;
    IF eski_ucret IS NULL THEN
        RAISE EXCEPTION 'Ücret kaydı bulunamadı';
    END IF;

    UPDATE hizmet
    SET ücret = yeni_ucret
    WHERE hizmet.hizmet_id = hizmet_ucret_guncelle.hizmet_id;

	RAISE NOTICE 'Hizmet fiyatı güncellendi. Hizmet ID: %, Eski Fiyat: %, Yeni Fiyat: %', hizmet_id, eski_ucret, yeni_ucret;
END;
$$;


ALTER PROCEDURE public.hizmet_ucret_guncelle(IN hizmet_id integer, IN yeni_ucret real) OWNER TO postgres;

--
-- TOC entry 264 (class 1255 OID 25199)
-- Name: hizmet_ucret_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.hizmet_ucret_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.ücret <= 0 THEN
        RAISE EXCEPTION 'Ücret alanı 0 dan büyük olmalıdır';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.hizmet_ucret_kontrol() OWNER TO postgres;

--
-- TOC entry 287 (class 1255 OID 25238)
-- Name: musait_oda_bul(date, date); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.musait_oda_bul(giris date, cikis date) RETURNS TABLE(oda_id integer, oda_ismi character varying, oda_tipi integer, kat smallint, "gecelik_ücret" real)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT oda.oda_id, oda.oda_ismi, oda.oda_tipi, oda.kat, oda.gecelik_ücret
    FROM oda
    LEFT JOIN rezervasyon ON rezervasyon.oda_id = oda.oda_id
   	AND rezervasyon.giriş_tarihi < cikis
    AND rezervasyon.çıkış_tarihi > giris
    WHERE rezervasyon.oda_id IS NULL;
    
END;
$$;


ALTER FUNCTION public.musait_oda_bul(giris date, cikis date) OWNER TO postgres;

--
-- TOC entry 261 (class 1255 OID 25240)
-- Name: oda_durum_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.oda_durum_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.durum NOT IN ('dolu', 'boş') THEN
        RAISE EXCEPTION 'Durum alanı sadece dolu veya boş değerlerinden birini alabilir';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.oda_durum_kontrol() OWNER TO postgres;

--
-- TOC entry 270 (class 1255 OID 25212)
-- Name: oda_gecelik_ucret_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.oda_gecelik_ucret_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.gecelik_ücret <= 0 THEN
        RAISE EXCEPTION 'Gecelik_ücret alanı 0 dan büyük olmalıdır';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.oda_gecelik_ucret_kontrol() OWNER TO postgres;

--
-- TOC entry 285 (class 1255 OID 25227)
-- Name: oda_promosyon_uygula(integer, integer); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.oda_promosyon_uygula(IN oda_id integer, IN promosyon_id integer)
    LANGUAGE plpgsql
    AS $$
DECLARE
    basla date;
    bitis date;
    indirim INT;
    eski_ucret REAL;
BEGIN
    SELECT başlama_tarihi, bitiş_tarihi, indirim_oranı
    INTO basla, bitis, indirim
    FROM promosyon
    WHERE promosyon.promosyon_id = oda_promosyon_uygula.promosyon_id;

    IF basla IS NULL THEN
        RAISE EXCEPTION 'Promosyon_id kaydı bulunamadı';
    END IF;

    IF CURRENT_DATE < basla OR CURRENT_DATE > bitis THEN
        RAISE EXCEPTION 'Bugünün tarihi % dır fakat promosyon % ile % tarihleri arasında geçerlidir', CURRENT_DATE, basla, bitis;
    END IF;

    SELECT gecelik_ücret INTO eski_ucret FROM oda WHERE oda.oda_id = oda_promosyon_uygula.oda_id;
    IF eski_ucret IS NULL THEN
        RAISE EXCEPTION 'Oda_id kaydı bulunamadı';
    END IF;

    UPDATE oda
    SET gecelik_ücret = gecelik_ücret * (1 - (indirim / 100.0))
    WHERE oda.oda_id = oda_promosyon_uygula.oda_id;

    RAISE NOTICE 'Ücret başarıyla güncellendi. Eski Ücret: %, Yeni Ücret: %',eski_ucret, eski_ucret * (1 - (indirim/100.0));
END;
$$;


ALTER PROCEDURE public.oda_promosyon_uygula(IN oda_id integer, IN promosyon_id integer) OWNER TO postgres;

--
-- TOC entry 269 (class 1255 OID 25210)
-- Name: oda_tipi_kapasite_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.oda_tipi_kapasite_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.kapasite <= 0 THEN
        RAISE EXCEPTION 'Kapasite alanı 0 dan büyük olmalıdır';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.oda_tipi_kapasite_kontrol() OWNER TO postgres;

--
-- TOC entry 284 (class 1255 OID 25229)
-- Name: ortalama_puan_hesapla(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.ortalama_puan_hesapla() RETURNS real
    LANGUAGE plpgsql
    AS $$
DECLARE
    ortalama_puan REAL;
BEGIN
    SELECT AVG(puan) INTO ortalama_puan FROM geri_bildirim;
    RETURN ortalama_puan;
END;
$$;


ALTER FUNCTION public.ortalama_puan_hesapla() OWNER TO postgres;

--
-- TOC entry 266 (class 1255 OID 25204)
-- Name: promosyon_indirim_orani_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.promosyon_indirim_orani_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.indirim_oranı < 0 OR NEW.indirim_oranı > 100 THEN
        RAISE EXCEPTION 'indirim_oranı 0 ile 100 arasında bir değer olmalıdır';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.promosyon_indirim_orani_kontrol() OWNER TO postgres;

--
-- TOC entry 265 (class 1255 OID 25201)
-- Name: promosyon_tarih_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.promosyon_tarih_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.başlama_tarihi >= NEW.bitiş_tarihi THEN
        RAISE EXCEPTION 'başlama_tarihi bitiş_tarihi değerinden önce olmalıdır';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.promosyon_tarih_kontrol() OWNER TO postgres;

--
-- TOC entry 260 (class 1255 OID 25192)
-- Name: rezervasyon_durum_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.rezervasyon_durum_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF NEW.durum NOT IN ('aktif', 'tamamlandı', 'iptal') THEN
        RAISE EXCEPTION 'Durum alanı sadece aktif, tamamlandı veya iptal değerlerinden birini alabilir';
    END IF;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.rezervasyon_durum_kontrol() OWNER TO postgres;

--
-- TOC entry 259 (class 1255 OID 25188)
-- Name: rezervasyon_tarih_kontrol(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.rezervasyon_tarih_kontrol() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
    IF NEW."giriş_tarihi" >= NEW."çıkış_tarihi" THEN
        RAISE EXCEPTION 'Giriş tarihi çıkış tarihinden sonra veya eşitse işlem yapılamaz';
    END IF;
    RETURN NEW;
END;

$$;


ALTER FUNCTION public.rezervasyon_tarih_kontrol() OWNER TO postgres;

--
-- TOC entry 288 (class 1255 OID 25237)
-- Name: yeni_rezervasyon_olustur(integer, date, date, integer); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.yeni_rezervasyon_olustur(IN istenen_oda_id integer, IN giris date, IN cikis date, IN istenen_kisi_id integer)
    LANGUAGE plpgsql
    AS $$
DECLARE
    bulunan_oda_id INT;
	bulunan_kisi_id INT;
BEGIN
    SELECT oda_id INTO bulunan_oda_id
    FROM musait_oda_bul(giris, cikis)
    WHERE oda_id = istenen_oda_id;

	SELECT kişi_id INTO bulunan_kisi_id FROM müşteri WHERE kişi_id = istenen_kisi_id;
    
    IF bulunan_oda_id IS NULL THEN
        RAISE EXCEPTION 'Oda_id bu tarihlerde müsait değil';
	ELSIF bulunan_kisi_id IS NULL THEN
        RAISE EXCEPTION 'Kişi_id müşteri tablosunda yok';
    ELSE
        INSERT INTO rezervasyon (kişi_id, oda_id, giriş_tarihi, çıkış_tarihi, durum)
		VALUES (istenen_kisi_id, istenen_oda_id, giris, cikis, 'aktif');
        
        UPDATE oda SET durum = 'dolu' WHERE oda_id = bulunan_oda_id;
        
        RAISE NOTICE 'Talep edilen rezervasyon % idli odada başarıyla oluşturuldu', istenen_oda_id;
    END IF;
END;
$$;


ALTER PROCEDURE public.yeni_rezervasyon_olustur(IN istenen_oda_id integer, IN giris date, IN cikis date, IN istenen_kisi_id integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 235 (class 1259 OID 24949)
-- Name: bakım_talebi; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."bakım_talebi" (
    talep_id integer NOT NULL,
    oda_id integer NOT NULL,
    talep_tarihi date NOT NULL,
    durum character varying(40)
);


ALTER TABLE public."bakım_talebi" OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 24948)
-- Name: bakım_talebi_talep_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."bakım_talebi_talep_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."bakım_talebi_talep_id_seq" OWNER TO postgres;

--
-- TOC entry 5097 (class 0 OID 0)
-- Dependencies: 234
-- Name: bakım_talebi_talep_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."bakım_talebi_talep_id_seq" OWNED BY public."bakım_talebi".talep_id;


--
-- TOC entry 224 (class 1259 OID 24803)
-- Name: etkinlik; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.etkinlik (
    etkinlik_id integer NOT NULL,
    etkinlik_ismi character varying(20) NOT NULL,
    etkinlik_tarihi date,
    kapasite integer
);


ALTER TABLE public.etkinlik OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 24802)
-- Name: etkinlik_etkinlik_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.etkinlik_etkinlik_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.etkinlik_etkinlik_id_seq OWNER TO postgres;

--
-- TOC entry 5098 (class 0 OID 0)
-- Dependencies: 223
-- Name: etkinlik_etkinlik_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.etkinlik_etkinlik_id_seq OWNED BY public.etkinlik.etkinlik_id;


--
-- TOC entry 242 (class 1259 OID 24997)
-- Name: fatura; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fatura (
    fatura_id integer NOT NULL,
    rezervasyon_id integer NOT NULL,
    "toplam_ücret" real NOT NULL,
    "ödeme_tarihi" date NOT NULL,
    "ödeme_yöntemi" character varying(20) NOT NULL,
    "açıklama" character varying(40)
);


ALTER TABLE public.fatura OWNER TO postgres;

--
-- TOC entry 241 (class 1259 OID 24996)
-- Name: fatura_fatura_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.fatura_fatura_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.fatura_fatura_id_seq OWNER TO postgres;

--
-- TOC entry 5099 (class 0 OID 0)
-- Dependencies: 241
-- Name: fatura_fatura_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.fatura_fatura_id_seq OWNED BY public.fatura.fatura_id;


--
-- TOC entry 244 (class 1259 OID 25029)
-- Name: geri_bildirim; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geri_bildirim (
    geri_bildirim_id integer NOT NULL,
    "kişi_id" integer NOT NULL,
    rezervasyon_id integer NOT NULL,
    yorum character varying(40) NOT NULL,
    puan smallint NOT NULL,
    CONSTRAINT puan_kontrol CHECK (((puan >= 0) AND (puan <= 10)))
);


ALTER TABLE public.geri_bildirim OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 25028)
-- Name: geri_bildirim_geri_bildirim_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.geri_bildirim_geri_bildirim_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.geri_bildirim_geri_bildirim_id_seq OWNER TO postgres;

--
-- TOC entry 5100 (class 0 OID 0)
-- Dependencies: 243
-- Name: geri_bildirim_geri_bildirim_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.geri_bildirim_geri_bildirim_id_seq OWNED BY public.geri_bildirim.geri_bildirim_id;


--
-- TOC entry 222 (class 1259 OID 24794)
-- Name: hizmet; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hizmet (
    hizmet_id integer NOT NULL,
    hizmet_ismi character varying(20) NOT NULL,
    "ücret" real
);


ALTER TABLE public.hizmet OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 24793)
-- Name: hizmet_hizmet_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.hizmet_hizmet_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.hizmet_hizmet_id_seq OWNER TO postgres;

--
-- TOC entry 5101 (class 0 OID 0)
-- Dependencies: 221
-- Name: hizmet_hizmet_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.hizmet_hizmet_id_seq OWNED BY public.hizmet.hizmet_id;


--
-- TOC entry 228 (class 1259 OID 24819)
-- Name: imkanlar; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.imkanlar (
    imkan_id integer NOT NULL,
    imkan_isimi character varying(20) NOT NULL
);


ALTER TABLE public.imkanlar OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 24818)
-- Name: imkanlar_imkan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.imkanlar_imkan_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.imkanlar_imkan_id_seq OWNER TO postgres;

--
-- TOC entry 5102 (class 0 OID 0)
-- Dependencies: 227
-- Name: imkanlar_imkan_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.imkanlar_imkan_id_seq OWNED BY public.imkanlar.imkan_id;


--
-- TOC entry 258 (class 1259 OID 25123)
-- Name: kişi; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."kişi" (
    "kişi_id" integer NOT NULL,
    isim character varying(20) NOT NULL,
    soyisim character varying(20) NOT NULL,
    telefon bigint NOT NULL,
    "kişi_tipi" character varying(10) NOT NULL
);


ALTER TABLE public."kişi" OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 25122)
-- Name: kişi_kişi_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."kişi_kişi_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."kişi_kişi_id_seq" OWNER TO postgres;

--
-- TOC entry 5103 (class 0 OID 0)
-- Dependencies: 257
-- Name: kişi_kişi_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."kişi_kişi_id_seq" OWNED BY public."kişi"."kişi_id";


--
-- TOC entry 218 (class 1259 OID 24780)
-- Name: müşteri; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."müşteri" (
    "kişi_id" integer NOT NULL,
    adres character varying(40) NOT NULL
);


ALTER TABLE public."müşteri" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 24779)
-- Name: müşteri_müşteri_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."müşteri_müşteri_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."müşteri_müşteri_id_seq" OWNER TO postgres;

--
-- TOC entry 5104 (class 0 OID 0)
-- Dependencies: 217
-- Name: müşteri_müşteri_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."müşteri_müşteri_id_seq" OWNED BY public."müşteri"."kişi_id";


--
-- TOC entry 233 (class 1259 OID 24915)
-- Name: oda; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.oda (
    oda_id integer NOT NULL,
    oda_ismi character varying(20) NOT NULL,
    oda_tipi integer NOT NULL,
    kat smallint NOT NULL,
    "gecelik_ücret" real NOT NULL,
    durum character varying(40)
);


ALTER TABLE public.oda OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 24962)
-- Name: oda_etkinlik; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.oda_etkinlik (
    oda_id integer NOT NULL,
    etkinlik_id integer NOT NULL
);


ALTER TABLE public.oda_etkinlik OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 24961)
-- Name: oda_etkinlik_etkinlik_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.oda_etkinlik_etkinlik_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.oda_etkinlik_etkinlik_id_seq OWNER TO postgres;

--
-- TOC entry 5105 (class 0 OID 0)
-- Dependencies: 237
-- Name: oda_etkinlik_etkinlik_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.oda_etkinlik_etkinlik_id_seq OWNED BY public.oda_etkinlik.etkinlik_id;


--
-- TOC entry 236 (class 1259 OID 24960)
-- Name: oda_etkinlik_oda_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.oda_etkinlik_oda_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.oda_etkinlik_oda_id_seq OWNER TO postgres;

--
-- TOC entry 5106 (class 0 OID 0)
-- Dependencies: 236
-- Name: oda_etkinlik_oda_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.oda_etkinlik_oda_id_seq OWNED BY public.oda_etkinlik.oda_id;


--
-- TOC entry 231 (class 1259 OID 24890)
-- Name: oda_imkanlar; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.oda_imkanlar (
    odatipi_id integer NOT NULL,
    imkan_id integer NOT NULL
);


ALTER TABLE public.oda_imkanlar OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 24889)
-- Name: oda_imkanlar_imkan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.oda_imkanlar_imkan_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.oda_imkanlar_imkan_id_seq OWNER TO postgres;

--
-- TOC entry 5107 (class 0 OID 0)
-- Dependencies: 230
-- Name: oda_imkanlar_imkan_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.oda_imkanlar_imkan_id_seq OWNED BY public.oda_imkanlar.imkan_id;


--
-- TOC entry 229 (class 1259 OID 24888)
-- Name: oda_imkanlar_odatipi_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.oda_imkanlar_odatipi_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.oda_imkanlar_odatipi_id_seq OWNER TO postgres;

--
-- TOC entry 5108 (class 0 OID 0)
-- Dependencies: 229
-- Name: oda_imkanlar_odatipi_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.oda_imkanlar_odatipi_id_seq OWNED BY public.oda_imkanlar.odatipi_id;


--
-- TOC entry 232 (class 1259 OID 24914)
-- Name: oda_oda_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.oda_oda_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.oda_oda_id_seq OWNER TO postgres;

--
-- TOC entry 5109 (class 0 OID 0)
-- Dependencies: 232
-- Name: oda_oda_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.oda_oda_id_seq OWNED BY public.oda.oda_id;


--
-- TOC entry 226 (class 1259 OID 24810)
-- Name: oda_tipi; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.oda_tipi (
    odatipi_id integer NOT NULL,
    tip_ismi character varying NOT NULL,
    kapasite integer
);


ALTER TABLE public.oda_tipi OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 24809)
-- Name: oda_tipi_odatipi_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.oda_tipi_odatipi_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.oda_tipi_odatipi_id_seq OWNER TO postgres;

--
-- TOC entry 5110 (class 0 OID 0)
-- Dependencies: 225
-- Name: oda_tipi_odatipi_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.oda_tipi_odatipi_id_seq OWNED BY public.oda_tipi.odatipi_id;


--
-- TOC entry 256 (class 1259 OID 25105)
-- Name: oda_çalışan; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."oda_çalışan" (
    "kişi_id" integer NOT NULL,
    oda_id integer NOT NULL
);


ALTER TABLE public."oda_çalışan" OWNER TO postgres;

--
-- TOC entry 255 (class 1259 OID 25104)
-- Name: oda_çalışan_oda_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."oda_çalışan_oda_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."oda_çalışan_oda_id_seq" OWNER TO postgres;

--
-- TOC entry 5111 (class 0 OID 0)
-- Dependencies: 255
-- Name: oda_çalışan_oda_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."oda_çalışan_oda_id_seq" OWNED BY public."oda_çalışan".oda_id;


--
-- TOC entry 254 (class 1259 OID 25103)
-- Name: oda_çalışan_çalışan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."oda_çalışan_çalışan_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."oda_çalışan_çalışan_id_seq" OWNER TO postgres;

--
-- TOC entry 5112 (class 0 OID 0)
-- Dependencies: 254
-- Name: oda_çalışan_çalışan_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."oda_çalışan_çalışan_id_seq" OWNED BY public."oda_çalışan"."kişi_id";


--
-- TOC entry 216 (class 1259 OID 24773)
-- Name: promosyon; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.promosyon (
    promosyon_id integer NOT NULL,
    promosyon_ismi character varying(40) NOT NULL,
    "indirim_oranı" integer,
    "başlama_tarihi" date,
    "bitiş_tarihi" date
);


ALTER TABLE public.promosyon OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 24772)
-- Name: promosyon_promosyon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.promosyon_promosyon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.promosyon_promosyon_id_seq OWNER TO postgres;

--
-- TOC entry 5113 (class 0 OID 0)
-- Dependencies: 215
-- Name: promosyon_promosyon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.promosyon_promosyon_id_seq OWNED BY public.promosyon.promosyon_id;


--
-- TOC entry 240 (class 1259 OID 24980)
-- Name: rezervasyon; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rezervasyon (
    rezervasyon_id integer NOT NULL,
    "kişi_id" integer NOT NULL,
    oda_id integer NOT NULL,
    "giriş_tarihi" date NOT NULL,
    "çıkış_tarihi" date NOT NULL,
    durum character varying(40)
);


ALTER TABLE public.rezervasyon OWNER TO postgres;

--
-- TOC entry 250 (class 1259 OID 25067)
-- Name: rezervasyon_hizmet; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rezervasyon_hizmet (
    rezervasyon_id integer NOT NULL,
    hizmet_id integer NOT NULL
);


ALTER TABLE public.rezervasyon_hizmet OWNER TO postgres;

--
-- TOC entry 249 (class 1259 OID 25066)
-- Name: rezervasyon_hizmet_hizmet_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.rezervasyon_hizmet_hizmet_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.rezervasyon_hizmet_hizmet_id_seq OWNER TO postgres;

--
-- TOC entry 5114 (class 0 OID 0)
-- Dependencies: 249
-- Name: rezervasyon_hizmet_hizmet_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.rezervasyon_hizmet_hizmet_id_seq OWNED BY public.rezervasyon_hizmet.hizmet_id;


--
-- TOC entry 248 (class 1259 OID 25065)
-- Name: rezervasyon_hizmet_rezervasyon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.rezervasyon_hizmet_rezervasyon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.rezervasyon_hizmet_rezervasyon_id_seq OWNER TO postgres;

--
-- TOC entry 5115 (class 0 OID 0)
-- Dependencies: 248
-- Name: rezervasyon_hizmet_rezervasyon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.rezervasyon_hizmet_rezervasyon_id_seq OWNED BY public.rezervasyon_hizmet.rezervasyon_id;


--
-- TOC entry 247 (class 1259 OID 25048)
-- Name: rezervasyon_promosyon; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rezervasyon_promosyon (
    promosyon_id integer NOT NULL,
    rezervasyon_id integer NOT NULL
);


ALTER TABLE public.rezervasyon_promosyon OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 25046)
-- Name: rezervasyon_promosyon_promosyon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.rezervasyon_promosyon_promosyon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.rezervasyon_promosyon_promosyon_id_seq OWNER TO postgres;

--
-- TOC entry 5116 (class 0 OID 0)
-- Dependencies: 245
-- Name: rezervasyon_promosyon_promosyon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.rezervasyon_promosyon_promosyon_id_seq OWNED BY public.rezervasyon_promosyon.promosyon_id;


--
-- TOC entry 246 (class 1259 OID 25047)
-- Name: rezervasyon_promosyon_rezervasyon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.rezervasyon_promosyon_rezervasyon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.rezervasyon_promosyon_rezervasyon_id_seq OWNER TO postgres;

--
-- TOC entry 5117 (class 0 OID 0)
-- Dependencies: 246
-- Name: rezervasyon_promosyon_rezervasyon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.rezervasyon_promosyon_rezervasyon_id_seq OWNED BY public.rezervasyon_promosyon.rezervasyon_id;


--
-- TOC entry 239 (class 1259 OID 24979)
-- Name: rezervasyon_rezervasyon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.rezervasyon_rezervasyon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.rezervasyon_rezervasyon_id_seq OWNER TO postgres;

--
-- TOC entry 5118 (class 0 OID 0)
-- Dependencies: 239
-- Name: rezervasyon_rezervasyon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.rezervasyon_rezervasyon_id_seq OWNED BY public.rezervasyon.rezervasyon_id;


--
-- TOC entry 220 (class 1259 OID 24787)
-- Name: çalışan; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."çalışan" (
    "kişi_id" integer NOT NULL,
    kimlik_no bigint NOT NULL,
    pozisyon character varying(20)
);


ALTER TABLE public."çalışan" OWNER TO postgres;

--
-- TOC entry 253 (class 1259 OID 25086)
-- Name: çalışan_hizmet; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."çalışan_hizmet" (
    "kişi_id" integer NOT NULL,
    hizmet_id integer NOT NULL
);


ALTER TABLE public."çalışan_hizmet" OWNER TO postgres;

--
-- TOC entry 252 (class 1259 OID 25085)
-- Name: çalışan_hizmet_hizmet_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."çalışan_hizmet_hizmet_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."çalışan_hizmet_hizmet_id_seq" OWNER TO postgres;

--
-- TOC entry 5119 (class 0 OID 0)
-- Dependencies: 252
-- Name: çalışan_hizmet_hizmet_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."çalışan_hizmet_hizmet_id_seq" OWNED BY public."çalışan_hizmet".hizmet_id;


--
-- TOC entry 251 (class 1259 OID 25084)
-- Name: çalışan_hizmet_çalışan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."çalışan_hizmet_çalışan_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."çalışan_hizmet_çalışan_id_seq" OWNER TO postgres;

--
-- TOC entry 5120 (class 0 OID 0)
-- Dependencies: 251
-- Name: çalışan_hizmet_çalışan_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."çalışan_hizmet_çalışan_id_seq" OWNED BY public."çalışan_hizmet"."kişi_id";


--
-- TOC entry 219 (class 1259 OID 24786)
-- Name: çalışan_çalışan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."çalışan_çalışan_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."çalışan_çalışan_id_seq" OWNER TO postgres;

--
-- TOC entry 5121 (class 0 OID 0)
-- Dependencies: 219
-- Name: çalışan_çalışan_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."çalışan_çalışan_id_seq" OWNED BY public."çalışan"."kişi_id";


--
-- TOC entry 4813 (class 2604 OID 24952)
-- Name: bakım_talebi talep_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."bakım_talebi" ALTER COLUMN talep_id SET DEFAULT nextval('public."bakım_talebi_talep_id_seq"'::regclass);


--
-- TOC entry 4807 (class 2604 OID 24806)
-- Name: etkinlik etkinlik_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.etkinlik ALTER COLUMN etkinlik_id SET DEFAULT nextval('public.etkinlik_etkinlik_id_seq'::regclass);


--
-- TOC entry 4817 (class 2604 OID 25000)
-- Name: fatura fatura_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fatura ALTER COLUMN fatura_id SET DEFAULT nextval('public.fatura_fatura_id_seq'::regclass);


--
-- TOC entry 4818 (class 2604 OID 25032)
-- Name: geri_bildirim geri_bildirim_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geri_bildirim ALTER COLUMN geri_bildirim_id SET DEFAULT nextval('public.geri_bildirim_geri_bildirim_id_seq'::regclass);


--
-- TOC entry 4806 (class 2604 OID 24797)
-- Name: hizmet hizmet_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hizmet ALTER COLUMN hizmet_id SET DEFAULT nextval('public.hizmet_hizmet_id_seq'::regclass);


--
-- TOC entry 4809 (class 2604 OID 24822)
-- Name: imkanlar imkan_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.imkanlar ALTER COLUMN imkan_id SET DEFAULT nextval('public.imkanlar_imkan_id_seq'::regclass);


--
-- TOC entry 4827 (class 2604 OID 25126)
-- Name: kişi kişi_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."kişi" ALTER COLUMN "kişi_id" SET DEFAULT nextval('public."kişi_kişi_id_seq"'::regclass);


--
-- TOC entry 4804 (class 2604 OID 24783)
-- Name: müşteri kişi_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."müşteri" ALTER COLUMN "kişi_id" SET DEFAULT nextval('public."müşteri_müşteri_id_seq"'::regclass);


--
-- TOC entry 4812 (class 2604 OID 24918)
-- Name: oda oda_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda ALTER COLUMN oda_id SET DEFAULT nextval('public.oda_oda_id_seq'::regclass);


--
-- TOC entry 4814 (class 2604 OID 24965)
-- Name: oda_etkinlik oda_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_etkinlik ALTER COLUMN oda_id SET DEFAULT nextval('public.oda_etkinlik_oda_id_seq'::regclass);


--
-- TOC entry 4815 (class 2604 OID 24966)
-- Name: oda_etkinlik etkinlik_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_etkinlik ALTER COLUMN etkinlik_id SET DEFAULT nextval('public.oda_etkinlik_etkinlik_id_seq'::regclass);


--
-- TOC entry 4810 (class 2604 OID 24893)
-- Name: oda_imkanlar odatipi_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_imkanlar ALTER COLUMN odatipi_id SET DEFAULT nextval('public.oda_imkanlar_odatipi_id_seq'::regclass);


--
-- TOC entry 4811 (class 2604 OID 24894)
-- Name: oda_imkanlar imkan_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_imkanlar ALTER COLUMN imkan_id SET DEFAULT nextval('public.oda_imkanlar_imkan_id_seq'::regclass);


--
-- TOC entry 4808 (class 2604 OID 24813)
-- Name: oda_tipi odatipi_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_tipi ALTER COLUMN odatipi_id SET DEFAULT nextval('public.oda_tipi_odatipi_id_seq'::regclass);


--
-- TOC entry 4825 (class 2604 OID 25108)
-- Name: oda_çalışan kişi_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."oda_çalışan" ALTER COLUMN "kişi_id" SET DEFAULT nextval('public."oda_çalışan_çalışan_id_seq"'::regclass);


--
-- TOC entry 4826 (class 2604 OID 25109)
-- Name: oda_çalışan oda_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."oda_çalışan" ALTER COLUMN oda_id SET DEFAULT nextval('public."oda_çalışan_oda_id_seq"'::regclass);


--
-- TOC entry 4803 (class 2604 OID 24776)
-- Name: promosyon promosyon_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.promosyon ALTER COLUMN promosyon_id SET DEFAULT nextval('public.promosyon_promosyon_id_seq'::regclass);


--
-- TOC entry 4816 (class 2604 OID 24983)
-- Name: rezervasyon rezervasyon_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon ALTER COLUMN rezervasyon_id SET DEFAULT nextval('public.rezervasyon_rezervasyon_id_seq'::regclass);


--
-- TOC entry 4821 (class 2604 OID 25070)
-- Name: rezervasyon_hizmet rezervasyon_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_hizmet ALTER COLUMN rezervasyon_id SET DEFAULT nextval('public.rezervasyon_hizmet_rezervasyon_id_seq'::regclass);


--
-- TOC entry 4822 (class 2604 OID 25071)
-- Name: rezervasyon_hizmet hizmet_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_hizmet ALTER COLUMN hizmet_id SET DEFAULT nextval('public.rezervasyon_hizmet_hizmet_id_seq'::regclass);


--
-- TOC entry 4819 (class 2604 OID 25051)
-- Name: rezervasyon_promosyon promosyon_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_promosyon ALTER COLUMN promosyon_id SET DEFAULT nextval('public.rezervasyon_promosyon_promosyon_id_seq'::regclass);


--
-- TOC entry 4820 (class 2604 OID 25052)
-- Name: rezervasyon_promosyon rezervasyon_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_promosyon ALTER COLUMN rezervasyon_id SET DEFAULT nextval('public.rezervasyon_promosyon_rezervasyon_id_seq'::regclass);


--
-- TOC entry 4805 (class 2604 OID 24790)
-- Name: çalışan kişi_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."çalışan" ALTER COLUMN "kişi_id" SET DEFAULT nextval('public."çalışan_çalışan_id_seq"'::regclass);


--
-- TOC entry 4823 (class 2604 OID 25089)
-- Name: çalışan_hizmet kişi_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."çalışan_hizmet" ALTER COLUMN "kişi_id" SET DEFAULT nextval('public."çalışan_hizmet_çalışan_id_seq"'::regclass);


--
-- TOC entry 4824 (class 2604 OID 25090)
-- Name: çalışan_hizmet hizmet_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."çalışan_hizmet" ALTER COLUMN hizmet_id SET DEFAULT nextval('public."çalışan_hizmet_hizmet_id_seq"'::regclass);


--
-- TOC entry 5068 (class 0 OID 24949)
-- Dependencies: 235
-- Data for Name: bakım_talebi; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."bakım_talebi" (talep_id, oda_id, talep_tarihi, durum) FROM stdin;
\.


--
-- TOC entry 5057 (class 0 OID 24803)
-- Dependencies: 224
-- Data for Name: etkinlik; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.etkinlik (etkinlik_id, etkinlik_ismi, etkinlik_tarihi, kapasite) FROM stdin;
7	a	2020-01-01	6
\.


--
-- TOC entry 5075 (class 0 OID 24997)
-- Dependencies: 242
-- Data for Name: fatura; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.fatura (fatura_id, rezervasyon_id, "toplam_ücret", "ödeme_tarihi", "ödeme_yöntemi", "açıklama") FROM stdin;
\.


--
-- TOC entry 5077 (class 0 OID 25029)
-- Dependencies: 244
-- Data for Name: geri_bildirim; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.geri_bildirim (geri_bildirim_id, "kişi_id", rezervasyon_id, yorum, puan) FROM stdin;
6	12	16	asd	5
\.


--
-- TOC entry 5055 (class 0 OID 24794)
-- Dependencies: 222
-- Data for Name: hizmet; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.hizmet (hizmet_id, hizmet_ismi, "ücret") FROM stdin;
2	cc	55
\.


--
-- TOC entry 5061 (class 0 OID 24819)
-- Dependencies: 228
-- Data for Name: imkanlar; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.imkanlar (imkan_id, imkan_isimi) FROM stdin;
\.


--
-- TOC entry 5091 (class 0 OID 25123)
-- Dependencies: 258
-- Data for Name: kişi; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."kişi" ("kişi_id", isim, soyisim, telefon, "kişi_tipi") FROM stdin;
12	a	a	5555555555	müşteri
\.


--
-- TOC entry 5051 (class 0 OID 24780)
-- Dependencies: 218
-- Data for Name: müşteri; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."müşteri" ("kişi_id", adres) FROM stdin;
12	a
\.


--
-- TOC entry 5066 (class 0 OID 24915)
-- Dependencies: 233
-- Data for Name: oda; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.oda (oda_id, oda_ismi, oda_tipi, kat, "gecelik_ücret", durum) FROM stdin;
3	o	2	5	100	a
4	z	2	2	25	dolu
\.


--
-- TOC entry 5071 (class 0 OID 24962)
-- Dependencies: 238
-- Data for Name: oda_etkinlik; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.oda_etkinlik (oda_id, etkinlik_id) FROM stdin;
\.


--
-- TOC entry 5064 (class 0 OID 24890)
-- Dependencies: 231
-- Data for Name: oda_imkanlar; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.oda_imkanlar (odatipi_id, imkan_id) FROM stdin;
\.


--
-- TOC entry 5059 (class 0 OID 24810)
-- Dependencies: 226
-- Data for Name: oda_tipi; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.oda_tipi (odatipi_id, tip_ismi, kapasite) FROM stdin;
2	t	5
\.


--
-- TOC entry 5089 (class 0 OID 25105)
-- Dependencies: 256
-- Data for Name: oda_çalışan; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."oda_çalışan" ("kişi_id", oda_id) FROM stdin;
\.


--
-- TOC entry 5049 (class 0 OID 24773)
-- Dependencies: 216
-- Data for Name: promosyon; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.promosyon (promosyon_id, promosyon_ismi, "indirim_oranı", "başlama_tarihi", "bitiş_tarihi") FROM stdin;
10	p	50	2020-01-01	2020-01-02
11	w	50	2024-11-01	2024-12-30
\.


--
-- TOC entry 5073 (class 0 OID 24980)
-- Dependencies: 240
-- Data for Name: rezervasyon; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.rezervasyon (rezervasyon_id, "kişi_id", oda_id, "giriş_tarihi", "çıkış_tarihi", durum) FROM stdin;
16	12	3	2020-01-01	2020-01-02	aktif
21	12	4	2021-01-01	2021-01-02	aktif
22	12	4	2022-02-01	2022-02-02	aktif
\.


--
-- TOC entry 5083 (class 0 OID 25067)
-- Dependencies: 250
-- Data for Name: rezervasyon_hizmet; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.rezervasyon_hizmet (rezervasyon_id, hizmet_id) FROM stdin;
\.


--
-- TOC entry 5080 (class 0 OID 25048)
-- Dependencies: 247
-- Data for Name: rezervasyon_promosyon; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.rezervasyon_promosyon (promosyon_id, rezervasyon_id) FROM stdin;
\.


--
-- TOC entry 5053 (class 0 OID 24787)
-- Dependencies: 220
-- Data for Name: çalışan; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."çalışan" ("kişi_id", kimlik_no, pozisyon) FROM stdin;
\.


--
-- TOC entry 5086 (class 0 OID 25086)
-- Dependencies: 253
-- Data for Name: çalışan_hizmet; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."çalışan_hizmet" ("kişi_id", hizmet_id) FROM stdin;
\.


--
-- TOC entry 5122 (class 0 OID 0)
-- Dependencies: 234
-- Name: bakım_talebi_talep_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."bakım_talebi_talep_id_seq"', 4, true);


--
-- TOC entry 5123 (class 0 OID 0)
-- Dependencies: 223
-- Name: etkinlik_etkinlik_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.etkinlik_etkinlik_id_seq', 7, true);


--
-- TOC entry 5124 (class 0 OID 0)
-- Dependencies: 241
-- Name: fatura_fatura_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.fatura_fatura_id_seq', 6, true);


--
-- TOC entry 5125 (class 0 OID 0)
-- Dependencies: 243
-- Name: geri_bildirim_geri_bildirim_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.geri_bildirim_geri_bildirim_id_seq', 7, true);


--
-- TOC entry 5126 (class 0 OID 0)
-- Dependencies: 221
-- Name: hizmet_hizmet_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.hizmet_hizmet_id_seq', 3, true);


--
-- TOC entry 5127 (class 0 OID 0)
-- Dependencies: 227
-- Name: imkanlar_imkan_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.imkanlar_imkan_id_seq', 1, false);


--
-- TOC entry 5128 (class 0 OID 0)
-- Dependencies: 257
-- Name: kişi_kişi_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."kişi_kişi_id_seq"', 12, true);


--
-- TOC entry 5129 (class 0 OID 0)
-- Dependencies: 217
-- Name: müşteri_müşteri_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."müşteri_müşteri_id_seq"', 1, false);


--
-- TOC entry 5130 (class 0 OID 0)
-- Dependencies: 237
-- Name: oda_etkinlik_etkinlik_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.oda_etkinlik_etkinlik_id_seq', 1, false);


--
-- TOC entry 5131 (class 0 OID 0)
-- Dependencies: 236
-- Name: oda_etkinlik_oda_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.oda_etkinlik_oda_id_seq', 1, false);


--
-- TOC entry 5132 (class 0 OID 0)
-- Dependencies: 230
-- Name: oda_imkanlar_imkan_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.oda_imkanlar_imkan_id_seq', 1, false);


--
-- TOC entry 5133 (class 0 OID 0)
-- Dependencies: 229
-- Name: oda_imkanlar_odatipi_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.oda_imkanlar_odatipi_id_seq', 1, false);


--
-- TOC entry 5134 (class 0 OID 0)
-- Dependencies: 232
-- Name: oda_oda_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.oda_oda_id_seq', 8, true);


--
-- TOC entry 5135 (class 0 OID 0)
-- Dependencies: 225
-- Name: oda_tipi_odatipi_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.oda_tipi_odatipi_id_seq', 2, true);


--
-- TOC entry 5136 (class 0 OID 0)
-- Dependencies: 255
-- Name: oda_çalışan_oda_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."oda_çalışan_oda_id_seq"', 1, false);


--
-- TOC entry 5137 (class 0 OID 0)
-- Dependencies: 254
-- Name: oda_çalışan_çalışan_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."oda_çalışan_çalışan_id_seq"', 1, false);


--
-- TOC entry 5138 (class 0 OID 0)
-- Dependencies: 215
-- Name: promosyon_promosyon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.promosyon_promosyon_id_seq', 11, true);


--
-- TOC entry 5139 (class 0 OID 0)
-- Dependencies: 249
-- Name: rezervasyon_hizmet_hizmet_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.rezervasyon_hizmet_hizmet_id_seq', 1, false);


--
-- TOC entry 5140 (class 0 OID 0)
-- Dependencies: 248
-- Name: rezervasyon_hizmet_rezervasyon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.rezervasyon_hizmet_rezervasyon_id_seq', 1, false);


--
-- TOC entry 5141 (class 0 OID 0)
-- Dependencies: 245
-- Name: rezervasyon_promosyon_promosyon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.rezervasyon_promosyon_promosyon_id_seq', 1, false);


--
-- TOC entry 5142 (class 0 OID 0)
-- Dependencies: 246
-- Name: rezervasyon_promosyon_rezervasyon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.rezervasyon_promosyon_rezervasyon_id_seq', 1, false);


--
-- TOC entry 5143 (class 0 OID 0)
-- Dependencies: 239
-- Name: rezervasyon_rezervasyon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.rezervasyon_rezervasyon_id_seq', 22, true);


--
-- TOC entry 5144 (class 0 OID 0)
-- Dependencies: 252
-- Name: çalışan_hizmet_hizmet_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."çalışan_hizmet_hizmet_id_seq"', 1, false);


--
-- TOC entry 5145 (class 0 OID 0)
-- Dependencies: 251
-- Name: çalışan_hizmet_çalışan_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."çalışan_hizmet_çalışan_id_seq"', 1, false);


--
-- TOC entry 5146 (class 0 OID 0)
-- Dependencies: 219
-- Name: çalışan_çalışan_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."çalışan_çalışan_id_seq"', 1, false);


--
-- TOC entry 4849 (class 2606 OID 24954)
-- Name: bakım_talebi bakım_talebi_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."bakım_talebi"
    ADD CONSTRAINT "bakım_talebi_pkey" PRIMARY KEY (talep_id);


--
-- TOC entry 4839 (class 2606 OID 24808)
-- Name: etkinlik etkinlik_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.etkinlik
    ADD CONSTRAINT etkinlik_pkey PRIMARY KEY (etkinlik_id);


--
-- TOC entry 4855 (class 2606 OID 25002)
-- Name: fatura fatura_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fatura
    ADD CONSTRAINT fatura_pkey PRIMARY KEY (fatura_id);


--
-- TOC entry 4857 (class 2606 OID 25035)
-- Name: geri_bildirim geri_bildirim_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geri_bildirim
    ADD CONSTRAINT geri_bildirim_pkey PRIMARY KEY (geri_bildirim_id);


--
-- TOC entry 4837 (class 2606 OID 24801)
-- Name: hizmet hizmet_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hizmet
    ADD CONSTRAINT hizmet_pkey PRIMARY KEY (hizmet_id);


--
-- TOC entry 4843 (class 2606 OID 24824)
-- Name: imkanlar imkanlar_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.imkanlar
    ADD CONSTRAINT imkanlar_pkey PRIMARY KEY (imkan_id);


--
-- TOC entry 4867 (class 2606 OID 25128)
-- Name: kişi kişi_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."kişi"
    ADD CONSTRAINT "kişi_pkey" PRIMARY KEY ("kişi_id");


--
-- TOC entry 4829 (class 2606 OID 25139)
-- Name: kişi kişi_telefon_check; Type: CHECK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE public."kişi"
    ADD CONSTRAINT "kişi_telefon_check" CHECK (((telefon >= '5000000000'::bigint) AND (telefon <= '5999999999'::bigint))) NOT VALID;


--
-- TOC entry 4869 (class 2606 OID 25141)
-- Name: kişi kişi_telefon_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."kişi"
    ADD CONSTRAINT "kişi_telefon_key" UNIQUE (telefon);


--
-- TOC entry 4833 (class 2606 OID 24785)
-- Name: müşteri müşteri_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."müşteri"
    ADD CONSTRAINT "müşteri_pkey" PRIMARY KEY ("kişi_id");


--
-- TOC entry 4851 (class 2606 OID 24968)
-- Name: oda_etkinlik oda_etkinlik_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_etkinlik
    ADD CONSTRAINT oda_etkinlik_pkey PRIMARY KEY (oda_id, etkinlik_id);


--
-- TOC entry 4845 (class 2606 OID 24896)
-- Name: oda_imkanlar oda_imkanlar_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_imkanlar
    ADD CONSTRAINT oda_imkanlar_pkey PRIMARY KEY (odatipi_id, imkan_id);


--
-- TOC entry 4847 (class 2606 OID 24920)
-- Name: oda oda_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda
    ADD CONSTRAINT oda_pkey PRIMARY KEY (oda_id);


--
-- TOC entry 4841 (class 2606 OID 24817)
-- Name: oda_tipi oda_tipi_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_tipi
    ADD CONSTRAINT oda_tipi_pkey PRIMARY KEY (odatipi_id);


--
-- TOC entry 4865 (class 2606 OID 25111)
-- Name: oda_çalışan oda_çalışan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."oda_çalışan"
    ADD CONSTRAINT "oda_çalışan_pkey" PRIMARY KEY ("kişi_id", oda_id);


--
-- TOC entry 4831 (class 2606 OID 24778)
-- Name: promosyon promosyon_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.promosyon
    ADD CONSTRAINT promosyon_pkey PRIMARY KEY (promosyon_id);


--
-- TOC entry 4861 (class 2606 OID 25073)
-- Name: rezervasyon_hizmet rezervasyon_hizmet_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_hizmet
    ADD CONSTRAINT rezervasyon_hizmet_pkey PRIMARY KEY (rezervasyon_id, hizmet_id);


--
-- TOC entry 4853 (class 2606 OID 24985)
-- Name: rezervasyon rezervasyon_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon
    ADD CONSTRAINT rezervasyon_pkey PRIMARY KEY (rezervasyon_id);


--
-- TOC entry 4859 (class 2606 OID 25054)
-- Name: rezervasyon_promosyon rezervasyon_promosyon_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_promosyon
    ADD CONSTRAINT rezervasyon_promosyon_pkey PRIMARY KEY (promosyon_id, rezervasyon_id);


--
-- TOC entry 4863 (class 2606 OID 25092)
-- Name: çalışan_hizmet çalışan_hizmet_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."çalışan_hizmet"
    ADD CONSTRAINT "çalışan_hizmet_pkey" PRIMARY KEY ("kişi_id", hizmet_id);


--
-- TOC entry 4835 (class 2606 OID 24792)
-- Name: çalışan çalışan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."çalışan"
    ADD CONSTRAINT "çalışan_pkey" PRIMARY KEY ("kişi_id");


--
-- TOC entry 4898 (class 2620 OID 25209)
-- Name: bakım_talebi bakim_talebi_durum_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER bakim_talebi_durum_kontrol_trigger BEFORE INSERT OR UPDATE ON public."bakım_talebi" FOR EACH ROW EXECUTE FUNCTION public.bakim_talebi_durum_kontrol();


--
-- TOC entry 4899 (class 2620 OID 25207)
-- Name: bakım_talebi bakim_talep_tarihi_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER bakim_talep_tarihi_kontrol_trigger BEFORE INSERT OR UPDATE ON public."bakım_talebi" FOR EACH ROW EXECUTE FUNCTION public.bakim_talep_tarihi_kontrol();


--
-- TOC entry 4894 (class 2620 OID 25215)
-- Name: etkinlik etkinlik_kapasite_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER etkinlik_kapasite_kontrol_trigger BEFORE INSERT OR UPDATE ON public.etkinlik FOR EACH ROW EXECUTE FUNCTION public.etkinlik_kapasite_kontrol();


--
-- TOC entry 4902 (class 2620 OID 25195)
-- Name: fatura fatura_toplam_ucret_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER fatura_toplam_ucret_kontrol_trigger BEFORE INSERT OR UPDATE ON public.fatura FOR EACH ROW EXECUTE FUNCTION public.fatura_toplam_ucret_kontrol();


--
-- TOC entry 4903 (class 2620 OID 25198)
-- Name: fatura fatura_ödeme_yöntemi_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "fatura_ödeme_yöntemi_kontrol_trigger" BEFORE INSERT OR UPDATE ON public.fatura FOR EACH ROW EXECUTE FUNCTION public."fatura_ödeme_yöntemi_kontrol"();


--
-- TOC entry 4904 (class 2620 OID 25218)
-- Name: geri_bildirim geri_bildirim_puan_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER geri_bildirim_puan_kontrol_trigger BEFORE INSERT OR UPDATE ON public.geri_bildirim FOR EACH ROW EXECUTE FUNCTION public.geri_bildirim_puan_kontrol();


--
-- TOC entry 4893 (class 2620 OID 25200)
-- Name: hizmet hizmet_ucret_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER hizmet_ucret_kontrol_trigger BEFORE INSERT OR UPDATE ON public.hizmet FOR EACH ROW EXECUTE FUNCTION public.hizmet_ucret_kontrol();


--
-- TOC entry 4896 (class 2620 OID 25241)
-- Name: oda oda_durum_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER oda_durum_kontrol_trigger BEFORE INSERT OR UPDATE ON public.oda FOR EACH ROW EXECUTE FUNCTION public.oda_durum_kontrol();


--
-- TOC entry 4897 (class 2620 OID 25213)
-- Name: oda oda_gecelik_ucret_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER oda_gecelik_ucret_kontrol_trigger BEFORE INSERT OR UPDATE ON public.oda FOR EACH ROW EXECUTE FUNCTION public.oda_gecelik_ucret_kontrol();


--
-- TOC entry 4895 (class 2620 OID 25211)
-- Name: oda_tipi oda_tipi_kapasite_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER oda_tipi_kapasite_kontrol_trigger BEFORE INSERT OR UPDATE ON public.oda_tipi FOR EACH ROW EXECUTE FUNCTION public.oda_tipi_kapasite_kontrol();


--
-- TOC entry 4891 (class 2620 OID 25205)
-- Name: promosyon promosyon_indirim_orani_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER promosyon_indirim_orani_kontrol_trigger BEFORE INSERT OR UPDATE ON public.promosyon FOR EACH ROW EXECUTE FUNCTION public.promosyon_indirim_orani_kontrol();


--
-- TOC entry 4892 (class 2620 OID 25202)
-- Name: promosyon promosyon_tarih_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER promosyon_tarih_kontrol_trigger BEFORE INSERT OR UPDATE ON public.promosyon FOR EACH ROW EXECUTE FUNCTION public.promosyon_tarih_kontrol();


--
-- TOC entry 4900 (class 2620 OID 25193)
-- Name: rezervasyon rezervasyon_durum_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER rezervasyon_durum_kontrol_trigger BEFORE INSERT OR UPDATE ON public.rezervasyon FOR EACH ROW EXECUTE FUNCTION public.rezervasyon_durum_kontrol();


--
-- TOC entry 4901 (class 2620 OID 25191)
-- Name: rezervasyon rezervasyon_tarih_kontrol_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER rezervasyon_tarih_kontrol_trigger BEFORE INSERT OR UPDATE ON public.rezervasyon FOR EACH ROW EXECUTE FUNCTION public.rezervasyon_tarih_kontrol();


--
-- TOC entry 4875 (class 2606 OID 24955)
-- Name: bakım_talebi bakım_talebi_oda_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."bakım_talebi"
    ADD CONSTRAINT "bakım_talebi_oda_id_fkey" FOREIGN KEY (oda_id) REFERENCES public.oda(oda_id);


--
-- TOC entry 4880 (class 2606 OID 25003)
-- Name: fatura fatura_rezervasyon_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fatura
    ADD CONSTRAINT fatura_rezervasyon_id_fkey FOREIGN KEY (rezervasyon_id) REFERENCES public.rezervasyon(rezervasyon_id);


--
-- TOC entry 4881 (class 2606 OID 25036)
-- Name: geri_bildirim geri_bildirim_müşteri_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geri_bildirim
    ADD CONSTRAINT "geri_bildirim_müşteri_id_fkey" FOREIGN KEY ("kişi_id") REFERENCES public."müşteri"("kişi_id");


--
-- TOC entry 4882 (class 2606 OID 25041)
-- Name: geri_bildirim geri_bildirim_rezervasyon_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geri_bildirim
    ADD CONSTRAINT geri_bildirim_rezervasyon_id_fkey FOREIGN KEY (rezervasyon_id) REFERENCES public.rezervasyon(rezervasyon_id);


--
-- TOC entry 4870 (class 2606 OID 25242)
-- Name: müşteri müşteri_kişi_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."müşteri"
    ADD CONSTRAINT "müşteri_kişi_id_fkey" FOREIGN KEY ("kişi_id") REFERENCES public."kişi"("kişi_id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4876 (class 2606 OID 24974)
-- Name: oda_etkinlik oda_etkinlik_etkinlik_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_etkinlik
    ADD CONSTRAINT oda_etkinlik_etkinlik_id_fkey FOREIGN KEY (etkinlik_id) REFERENCES public.etkinlik(etkinlik_id);


--
-- TOC entry 4877 (class 2606 OID 24969)
-- Name: oda_etkinlik oda_etkinlik_oda_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_etkinlik
    ADD CONSTRAINT oda_etkinlik_oda_id_fkey FOREIGN KEY (oda_id) REFERENCES public.oda(oda_id);


--
-- TOC entry 4872 (class 2606 OID 24902)
-- Name: oda_imkanlar oda_imkanlar_imkan_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_imkanlar
    ADD CONSTRAINT oda_imkanlar_imkan_id_fkey FOREIGN KEY (imkan_id) REFERENCES public.imkanlar(imkan_id);


--
-- TOC entry 4873 (class 2606 OID 24897)
-- Name: oda_imkanlar oda_imkanlar_odatipi_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda_imkanlar
    ADD CONSTRAINT oda_imkanlar_odatipi_id_fkey FOREIGN KEY (odatipi_id) REFERENCES public.oda_tipi(odatipi_id);


--
-- TOC entry 4874 (class 2606 OID 24921)
-- Name: oda oda_oda_tipi_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda
    ADD CONSTRAINT oda_oda_tipi_fkey FOREIGN KEY (oda_tipi) REFERENCES public.oda_tipi(odatipi_id);


--
-- TOC entry 4889 (class 2606 OID 25117)
-- Name: oda_çalışan oda_çalışan_oda_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."oda_çalışan"
    ADD CONSTRAINT "oda_çalışan_oda_id_fkey" FOREIGN KEY (oda_id) REFERENCES public.oda(oda_id);


--
-- TOC entry 4890 (class 2606 OID 25112)
-- Name: oda_çalışan oda_çalışan_çalışan_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."oda_çalışan"
    ADD CONSTRAINT "oda_çalışan_çalışan_id_fkey" FOREIGN KEY ("kişi_id") REFERENCES public."çalışan"("kişi_id");


--
-- TOC entry 4885 (class 2606 OID 25079)
-- Name: rezervasyon_hizmet rezervasyon_hizmet_hizmet_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_hizmet
    ADD CONSTRAINT rezervasyon_hizmet_hizmet_id_fkey FOREIGN KEY (hizmet_id) REFERENCES public.hizmet(hizmet_id);


--
-- TOC entry 4886 (class 2606 OID 25074)
-- Name: rezervasyon_hizmet rezervasyon_hizmet_rezervasyon_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_hizmet
    ADD CONSTRAINT rezervasyon_hizmet_rezervasyon_id_fkey FOREIGN KEY (rezervasyon_id) REFERENCES public.rezervasyon(rezervasyon_id);


--
-- TOC entry 4878 (class 2606 OID 24986)
-- Name: rezervasyon rezervasyon_müşteri_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon
    ADD CONSTRAINT "rezervasyon_müşteri_id_fkey" FOREIGN KEY ("kişi_id") REFERENCES public."müşteri"("kişi_id");


--
-- TOC entry 4879 (class 2606 OID 24991)
-- Name: rezervasyon rezervasyon_oda_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon
    ADD CONSTRAINT rezervasyon_oda_id_fkey FOREIGN KEY (oda_id) REFERENCES public.oda(oda_id);


--
-- TOC entry 4883 (class 2606 OID 25055)
-- Name: rezervasyon_promosyon rezervasyon_promosyon_promosyon_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_promosyon
    ADD CONSTRAINT rezervasyon_promosyon_promosyon_id_fkey FOREIGN KEY (promosyon_id) REFERENCES public.promosyon(promosyon_id);


--
-- TOC entry 4884 (class 2606 OID 25060)
-- Name: rezervasyon_promosyon rezervasyon_promosyon_rezervasyon_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rezervasyon_promosyon
    ADD CONSTRAINT rezervasyon_promosyon_rezervasyon_id_fkey FOREIGN KEY (rezervasyon_id) REFERENCES public.rezervasyon(rezervasyon_id);


--
-- TOC entry 4887 (class 2606 OID 25098)
-- Name: çalışan_hizmet çalışan_hizmet_hizmet_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."çalışan_hizmet"
    ADD CONSTRAINT "çalışan_hizmet_hizmet_id_fkey" FOREIGN KEY (hizmet_id) REFERENCES public.hizmet(hizmet_id);


--
-- TOC entry 4888 (class 2606 OID 25093)
-- Name: çalışan_hizmet çalışan_hizmet_çalışan_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."çalışan_hizmet"
    ADD CONSTRAINT "çalışan_hizmet_çalışan_id_fkey" FOREIGN KEY ("kişi_id") REFERENCES public."çalışan"("kişi_id");


--
-- TOC entry 4871 (class 2606 OID 25247)
-- Name: çalışan çalışan_kişi_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."çalışan"
    ADD CONSTRAINT "çalışan_kişi_id_fkey" FOREIGN KEY ("kişi_id") REFERENCES public."kişi"("kişi_id") ON UPDATE CASCADE ON DELETE CASCADE;


-- Completed on 2024-12-20 14:15:15

--
-- PostgreSQL database dump complete
--

