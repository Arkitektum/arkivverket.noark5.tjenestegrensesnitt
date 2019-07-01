using System;
using System.Collections.Generic;
using System.Linq;
using arkivverket.noark5tj.models;
using arkivverket.noark5tj.webapi.Services;

namespace arkivverket.noark5.tjenestegrensesnitt.eksempel.Services
{
    internal static class MockNoarkDatalayer
    {
        private static readonly Random Random = new Random();

        internal static List<ArkivskaperType> Arkivskaper = new List<ArkivskaperType>();
        internal static List<ArkivType> Arkiver = new List<ArkivType>();
        internal static List<ArkivdelType> Arkivdeler = new List<ArkivdelType>();
        internal static List<MappeType> Mapper = new List<MappeType>();
        internal static List<SaksmappeType> Saksmapper = new List<SaksmappeType>();
        internal static List<RegistreringType> Registreringer = new List<RegistreringType>();
        internal static List<DokumentbeskrivelseType> Dokumentbeskrivelser = new List<DokumentbeskrivelseType>();
        internal static List<DokumentobjektType> Dokumentobjekter = new List<DokumentobjektType>();
        internal static List<JournalpostType> Journalposter = new List<JournalpostType>();
        internal static List<DokumentlinkType> Dokumentlinker = new List<DokumentlinkType>();

        /// <summary>
        /// Number of examples to be generated of each type. The number of items in the example arrays should be the same size
        /// </summary>
        private static int NumberOfExamples = 10;
        private static DateTime FixedDate = new DateTime(2017, 2, 28);
        private static readonly string[] FirstNames = { "Emma", "William", "Oliver", "Aksel", "Maja", "Sofie", "Nora", "Emilie", "Filip", "Jakob" };
        private static readonly string[] Adjectives = { "allergisk", "begeistret", "bred", "flink", "fremmed", "høflig", "irritert", "klok", "gammel", "lunken" };


        static MockNoarkDatalayer()
        {
            ResetMockData();
        }

        internal static void ResetMockData()
        {
            Arkiver.Clear();
            Arkivdeler.Clear();
            Arkivskaper.Clear();
            Mapper.Clear();
            Saksmapper.Clear();
            Registreringer.Clear();
            Dokumentbeskrivelser.Clear();
            Dokumentobjekter.Clear();
            Journalposter.Clear();

            OpprettArkiver();
        }



        private static KlasseType OpprettKlasse(int i)
        {
            var klasseType = new KlasseType();
            klasseType.tittel = "Tittel" + i;
            klasseType.systemID = i + "_sysId";
            klasseType.beskrivelse = "Dette er en beskrivelse av" + i;
            klasseType.klasseID = "KlasseId " + i;
            klasseType.oppdatertDato = GetDato(i);
            klasseType.oppdatertDatoSpecified = true;
            klasseType.oppdatertAv = GetName(i);
            klasseType.referanseOppdatertAv = GetName(i);
            klasseType.opprettetDato = GetDato(i);
            klasseType.opprettetDatoSpecified = true;
            klasseType.opprettetAv = GetName(i);
            klasseType.referanseOpprettetAv = GetName(i);
            return klasseType;
        }
        
        private static DokumentbeskrivelseType OpprettDokumentbeskrivelse(string systemId)
        {
            DokumentbeskrivelseType dokumentbeskrivelse = new DokumentbeskrivelseType
            {
                systemID = systemId,
                tittel = GetRandomAdjective() + " dokument",
                beskrivelse = "beskrivelse av dokumentet",
                opprettetDato = DateTime.Now
            };

            dokumentbeskrivelse.dokumentobjekt = new DokumentobjektType[]
                {OpprettDokumentobjekt(GenerateUuuid(), true, dokumentbeskrivelse)};
            
            dokumentbeskrivelse.RepopulateHyperMedia();

            Dokumentbeskrivelser.Add(dokumentbeskrivelse);

            return dokumentbeskrivelse;
        }

        private static List<ArkivdelType> OpprettArkivdeler(ArkivType tilknyttetArkiv)
        {
            var arkivdelerTilknyttetArkiv = new List<ArkivdelType>();
            for (int i = 0; i <= 3; i++)
            {
                var arkivdel = OpprettArkivdel(Guid.NewGuid().ToString(), tilknyttetArkiv);
                arkivdelerTilknyttetArkiv.Add(arkivdel);
                Arkivdeler.Add(arkivdel);
            }

            return arkivdelerTilknyttetArkiv;
        }

        private static ArkivdelType OpprettArkivdel(string id, ArkivType tilknyttetArkiv)
        {
            ArkivdelType arkivdel = new ArkivdelType
            {
                systemID = id,
                tittel = "arkivdel - " + id,
                beskrivelse = "beskrivelse av arkivdel",
                arkivperiodeStartDatoSpecified = true,
                arkivperiodeStartDato = DateTime.Parse("2019-01-01"),
                opprettetDatoSpecified = true,
                opprettetDato = DateTime.Parse("2019-01-01"),
                arkivdelstatus = new ArkivdelstatusType
                {
                    kode = "A",
                    beskrivelse = "Aktiv periode"
                },
                arkiv = tilknyttetArkiv,
            };
            arkivdel.registrering = OpprettRegistreringer(arkivdel).ToArray();
            
            var mapper = OpprettMapper(arkivdel);
            mapper.AddRange(OpprettSaksmapper(arkivdel));
            arkivdel.mappe = mapper.ToArray();
            
            arkivdel.RepopulateHyperMedia();
            return arkivdel;
        }

        private static List<JournalpostType> OpprettJournalposter(ArkivdelType tilknyttetArkivdel, MappeType tilknyttetMappe)
        {
            var journalposterTilknyttetElement = new List<JournalpostType>();
            for (int i = 0; i <= 3; i++)
            {
                var journalpost = OpprettJournalpost(Guid.NewGuid().ToString(), tilknyttetArkivdel, tilknyttetMappe);
                journalposterTilknyttetElement.Add(journalpost);
                Journalposter.Add(journalpost);
            }

            return journalposterTilknyttetElement;
        }

        private static JournalpostType OpprettJournalpost(string id, ArkivdelType tilknyttetArkivdel, MappeType tilknyttetMappe)
        {
            JournalpostType journalPost = new JournalpostType
            {
                systemID = id,
                opprettetDato = DateTime.Now,
                opprettetDatoSpecified = true,
                oppdatertDato = DateTime.Now,
                journaldato = DateTime.Now,
                tittel = "journalpost - " + id,
                oppdatertAv = "bruker",
                arkivdel = tilknyttetArkivdel,
                mappe = tilknyttetMappe,
            };

            journalPost.LinkList.Clear();
            journalPost.RepopulateHyperMedia();

            return journalPost;
        }

        private static DokumentobjektType OpprettDokumentobjekt(string id, bool isDokumentobjektMedReferanseFil, DokumentbeskrivelseType tilknyttetDokumentbeskrivelse)
        {
            DokumentobjektType dokumentObjekt = new DokumentobjektType();
            dokumentObjekt.systemID = id;
            dokumentObjekt.versjonsnummer = "1";
            dokumentObjekt.variantformat = new VariantformatType() { kode = "A", beskrivelse = "Arkivformat" };
            dokumentObjekt.format = new FormatType() { kode = "RA-PDF", beskrivelse = "PDF/A - ISO 19005-1:2005" };
            dokumentObjekt.opprettetDato = DateTime.Now;

            dokumentObjekt.dokumentbeskrivelse = tilknyttetDokumentbeskrivelse;

            if (isDokumentobjektMedReferanseFil)
                dokumentObjekt.referanseDokumentfil = BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/Dokumentobjekt/" + dokumentObjekt.systemID + "/referanseFil";

            dokumentObjekt.RepopulateHyperMedia();

            Dokumentobjekter.Add(dokumentObjekt);

            return dokumentObjekt;
        }


        private static IEnumerable<MappeType> OpprettSaksmapper(ArkivdelType tilknyttetArkivdel)
        {
            var saksmapperTilknyttetArkivdel = new List<SaksmappeType>();
            for (int i = 1; i <= NumberOfExamples; i++)
            {
                var saksmappe = OpprettSaksmappe(GenerateUuuid(), tilknyttetArkivdel);
                saksmapperTilknyttetArkivdel.Add(saksmappe);
                Saksmapper.Add(saksmappe);
            }

            return saksmapperTilknyttetArkivdel;
        }

        private static SaksmappeType OpprettSaksmappe(string id, ArkivdelType tilknyttetArkivdel)
        {
            var randomNumber = RandomNumber(0,100);
            var saksmappe = new SaksmappeType
            {
                systemID = id,
                mappeID = $"100{randomNumber}/2017",
                tittel = GetRandomAdjective() + " saksmappe",
                opprettetDato = GetDato(RandomNumber(0,200)),
                opprettetDatoSpecified = true,
                oppdatertAv = GetName(),
                saksaar = "2017",
                sakssekvensnummer = randomNumber.ToString(),
                sakspart = OpprettSakspart(randomNumber),
                saksdato = GetDato(randomNumber),
                nasjonalidentifikator = OpprettNasjonalidentifikator(randomNumber),
                sekundaerklassifikasjon = OpprettSekundaerklassifikasjoner()
            };
            saksmappe.arkivdel = tilknyttetArkivdel;

            saksmappe.sakspart[0].RepopulateHyperMedia();
            saksmappe.RepopulateHyperMedia();
            return saksmappe;
        }

        private static KlasseType[] OpprettSekundaerklassifikasjoner()
        {
            var klasseTyper = new List<KlasseType>();

            for (int i = 1; i <= 2; i++)
            {
                klasseTyper.Add(OpprettKlasse(i));
            }
            return klasseTyper.ToArray();
        }

        private static List<RegistreringType> OpprettRegistreringer(ArkivdelType tilknyttetArkivdel)
        {
            List<RegistreringType> registreringerForArkivdel = new List<RegistreringType>();
            for (int i = 1; i <= NumberOfExamples; i++)
            {
                var registrering = OpprettRegistrering(i, tilknyttetArkivdel);
                registreringerForArkivdel.Add(registrering);
                Registreringer.Add(registrering);
            }
            return registreringerForArkivdel;
        }

        private static RegistreringType OpprettRegistrering(int index, ArkivdelType tilknyttetArkivdel)
        {
            var registrering = new RegistreringType()
            {
                LinkList = null,

                systemID = index.ToString(),
                oppdatertDato = GetDato(index),
                oppdatertDatoSpecified = true,
                opprettetDato = GetDato(index),
                opprettetDatoSpecified = true,
                opprettetAv = GetName(index),
                oppdatertAv = GetName(index),
                referanseOppdatertAv = GetName(index),
                referanseOpprettetAv = GetName(index),
                logg = null,

                arkivertDato = GetDato(index),
                arkivertDatoSpecified = true,
                arkivertAv = GetName(index),
                referanseArkivertAv = GetName(index),
                kassasjon = null,
                skjerming = new SkjermingType(),
                gradering = new GraderingType(),
                referanseArkivdel = null,
                klasse = null,
                mappe = null,
                arkivdel = tilknyttetArkivdel,
                nasjonalidentifikator = OpprettNasjonalidentifikator(index),
                
            };
            registrering.dokumentlink = OpprettDokumentlinker(registrering).ToArray();

            registrering.RepopulateHyperMedia();

            return registrering;
        }

        private static List<DokumentlinkType> OpprettDokumentlinker(RegistreringType tilknyttetRegistrering)
        {
            var dokumentlinkerTilknyttetRegistrering = new List<DokumentlinkType>();
            for (int i = 1; i <= NumberOfExamples; i++)
            {
                var registrering = OpprettDokumentlink(GenerateUuuid(), tilknyttetRegistrering);
                dokumentlinkerTilknyttetRegistrering.Add(registrering);
                Dokumentlinker.Add(registrering);
            }
            return dokumentlinkerTilknyttetRegistrering;
        }

        private static DokumentlinkType OpprettDokumentlink(string id, RegistreringType tilknyttetRegistrering)
        {
            var dokumentlink = new DokumentlinkType()
            {
                systemID = id,
                dokumentbeskrivelse = OpprettDokumentbeskrivelse(GenerateUuuid())
            };
            dokumentlink.registrering = tilknyttetRegistrering;

            dokumentlink.RepopulateHyperMedia();
            return dokumentlink;
        }


        private static AbstraktNasjonalidentifikatorType[] OpprettNasjonalidentifikator(int index)
        {
            bool opprettPersonId = index % 2 == 1;

            if (opprettPersonId)
            {
                return new AbstraktNasjonalidentifikatorType[]
                {
                    new PersonidentifikatorType()
                    {
                        foedselsnummer = "12334566778"
                    }
                };
            }
            return new AbstraktNasjonalidentifikatorType[]
            {
                new BygningType
                {
                    byggidentifikator = new ByggIdent() {bygningsNummer = "12345678"}
                }
            };
        }

        private static AbstraktSakspartType[] OpprettSakspart(int index)
        {
            bool opprettPersonSakspart = index % 2 == 1;

            if (opprettPersonSakspart)
            {
                return new AbstraktSakspartType[]
                {
                    new SakspartPersonType()
                    {
                        systemID = index.ToString(),
                        foedselsnummer = "12334566778",
                        navn = "Jan Johansen",
                        sakspartRolle = new SakspartRolleType()
                        {
                            kode = "KLI",
                            beskrivelse = "Klient"
                        }
                    }
                };
            }
            return new AbstraktSakspartType[]
            {
                new SakspartEnhetType()
                {
                    systemID = index.ToString(),
                    organisasjonsnummer = "998877665",
                    navn = "Advokatselskap AS",
                    sakspartRolle = new SakspartRolleType()
                    {
                        kode = "ADV",
                        beskrivelse = "Advokat"
                    }
                }
            };
        }

        internal static void DeleteSekundaerklassifikasjonFromSaksmappe(string id, KlasseType[] klasseTyper)
        {
            foreach (var klasseType in klasseTyper)
            {
                DeleteSekundaerklassifikasjonFromSaksmappe(id, klasseType.systemID);
            }
        }

        private static DateTime GetDato(int index)
        {
            return FixedDate.AddDays(-(index + 10));
        }

        private static string Tittel(string objektType, int index)
        {
            while (index > 10)
            {
                index = index - 10;
            }
            return $"{Adjectives[index - 1]} {objektType} nr. {index}";
        }

        private static string GetName(int index)
        {
            while (index > 10)
            {
                index = index - 10;
            }
            return FirstNames[index - 1];
        }

        private static string GetName()
        {
            return FirstNames[RandomNumber(0, FirstNames.Length-1)];
        }

        public static DokumentmediumType ElektroniskDokumentmedium = new DokumentmediumType
        {
            kode = "E",
            beskrivelse = "Elektronisk arkiv"
        };

        public static ArkivstatusType OpprettetArkivstatus = new ArkivstatusType
        {
            kode = "O",
            beskrivelse = "Opprettet"
        };

        public static ArkivstatusType AvsluttetArkivstatus = new ArkivstatusType
        {
            kode = "A",
            beskrivelse = "Avsluttet"
        };

        public static KlassifikasjonstypeType GbnKlassifikasjonstype = new KlassifikasjonstypeType
        {
            kode = "GBN",
            beskrivelse = "Gårds- og bruksnummer"
        };

        public static FormatType PdfFormat = new FormatType
        {
            kode = "RA-PDF",
            beskrivelse = "PDF/A - ISO 19005-1:2005"
        };

        public static VariantformatType ArkivFormat = new VariantformatType
        {
            kode = "A",
            beskrivelse = "Arkivformat"
        };

        private static ArkivskaperType[] OpprettArkivskaper()
        {
            List<ArkivskaperType> arkivskaperTyper = new List<ArkivskaperType>();
            var arkivskaper = new ArkivskaperType
            {
                systemID = GenerateUuuid(),
                arkivskaperID = RandomNumber(100, 1000).ToString(),
                arkivskaperNavn = GetRandomKommune(),
                beskrivelse = "Lorem ipsum",
                opprettetAv = "brukernavn",
                referanseOppdatertAv = GenerateUuuid()
            };
            arkivskaper.RepopulateHyperMedia();
            Arkivskaper.Add(arkivskaper); // add to global list
            arkivskaperTyper.Add(arkivskaper);

            return arkivskaperTyper.ToArray();
        }

        private static void OpprettArkiver()
        {
            for (int i = 0; i < 10; i++)
            {
                Arkiver.Add(OpprettArkiv((i + 1).ToString()));
            }
        }

        private static ArkivType OpprettArkiv(string systemId)
        {
            var arkiv = new ArkivType()
            {
                tittel = FirstLetterToUpper(GetRandomAdjective()) + " arkiv",
                arkivstatus = OpprettetArkivstatus,
                dokumentmedium = ElektroniskDokumentmedium,
                systemID = systemId,
                beskrivelse = "lorem ipsum " + GetRandomAdjective(),
                arkivskaper = OpprettArkivskaper(),
                opprettetAv = "brukernavn",
                referanseOpprettetAv = GenerateUuuid()

            };

            var randomNumber = RandomNumber(0, 100);
            if (randomNumber < 50)
            {
                arkiv.arkivstatus = AvsluttetArkivstatus;
                arkiv.avsluttetDato = DateTime.Now;
                arkiv.avsluttetDatoSpecified = true;
                arkiv.avsluttetAv = "brukernavn";
                arkiv.referanseAvsluttetAv = GenerateUuuid();
            }
            
            OpprettArkivdeler(arkiv).ToArray();

            arkiv.RepopulateHyperMedia();
            
            return arkiv;
        }

        /// <summary>
        /// Find a mappe by id. Uses the field systemID to search within.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MappeType GetMappeById(string id)
        {
            return Mapper.Find(m => m.systemID == id);
        }

        private static List<MappeType> OpprettMapper(ArkivdelType tilknyttetArkivdel)
        {
            var mapperTilknyttetArkivdel = new List<MappeType>();
            for (int i = 0; i < 10; i++)
            {
                var mappe = OpprettMappe((i + 1).ToString(), tilknyttetArkivdel);
                mapperTilknyttetArkivdel.Add(mappe);
                Mapper.Add(mappe);
            }

            return mapperTilknyttetArkivdel;
        }

        private static MappeType OpprettMappe(string id, ArkivdelType tilknyttetArkivdel)
        {
            MappeType m = new MappeType();
            m.tittel = GetRandomAdjective() + " testmappe " + id;
            m.offentligTittel = "Dette er en offentlig tittel ****";
            m.systemID = id;
            m.opprettetDato = DateTime.Now;
            m.opprettetDatoSpecified = true;
            m.opprettetAv = OpprettetAv();
            m.mappeID = id + "/2014";
            m.gradering = new GraderingType
            {
                graderingskode = new GraderingskodeType { kode = "B" },
                graderingsdato = DateTime.Now
            };
            m.klasse = new KlasseType()
            {
                klasseID = "12345678901", 
                tittel = "12345678901", 
                klassifikasjonssystem = new KlassifikasjonssystemType
                {
                    klassifikasjonstype = new KlassifikasjonstypeType { kode = "PNR", beskrivelse = "Personnr" }
                }
            }; 
            List<MerknadType> merknader = new List<MerknadType>
            {
                new MerknadType
                {
                    merknadstype = new MerknadstypeType {kode = "B"},
                    merknadstekst = "test"
                }
            };
            m.merknad = merknader.ToArray();
            m.virksomhetsspesifikkeMetadata = new virksomhetsspesifikkeMetadata()
            {
                eksempelfelt = "Nikita eksempelfelt " + id,
                henvisningdato = DateTime.Now.Subtract(TimeSpan.FromDays(RandomNumber(1, 500))),
                skoleaar = "2018/2019"
            };
            m.arkivdel = tilknyttetArkivdel;
            m.registrering = OpprettJournalposter(tilknyttetArkivdel, m).ToArray();
            m.LinkList.Clear();
            m.RepopulateHyperMedia();
            return m;
        }

        private static string OpprettetAv()
        {
            return FirstNames[RandomNumber(0, FirstNames.Length - 1)];
        }

        private static string GenerateUuuid()
        {
            return Guid.NewGuid().ToString();
        }

        private static int RandomNumber(int min, int max)
        {
            return Random.Next(min, max);
        }

        private static string GetRandomKommune()
        {
            return FirstLetterToUpper(GetRandomAdjective()) + " kommune";
        }

        private static string GetRandomAdjective()
        {
            return Adjectives[RandomNumber(0, Adjectives.Length - 1)];
        }

        private static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static ArkivType GetArkivById(string id)
        {
            return Arkiver.Find(a => a.systemID == id);
        }

        public static ArkivskaperType GetArkivskaperById(string id)
        {
            return Arkivskaper.Find(a => a.systemID == id);
        }

        public static SaksmappeType GetSaksmappeById(string id)
        {
            return Saksmapper.Find(s => s.systemID == id);
        }

        public static DokumentbeskrivelseType GetDokumentbeskrivelseById(string id)
        {
            return Dokumentbeskrivelser.Find(d => d.systemID == id);
        }

        public static JournalpostType GetJournalpostById(string id)
        {
            return Journalposter.Find(j => j.systemID == id);
        }

        public static DokumentobjektType GetDokumentobjektById(string id)
        {
            return Dokumentobjekter.Find(d => d.systemID == id);
        }

        public static void AddSekundaerklassifikasjonToSaksmappe(string saksmappeSystemId, KlasseType klasseType)
        {
            var funnet = false;
            if (klasseType != null)
            {
                foreach (var saksmappe in Saksmapper)
                {
                    if (saksmappe.systemID == saksmappeSystemId)
                    {
                        var sekundaerklassifikasjonerList = saksmappe.sekundaerklassifikasjon.ToList();
                        sekundaerklassifikasjonerList.Add(klasseType);
                        saksmappe.sekundaerklassifikasjon = sekundaerklassifikasjonerList.ToArray();
                        funnet = true;
                    }
                }
            }
            if (!funnet)
            {
                throw new ArgumentNullException("Saksmappen finnes ikke");
            }
        }

        public static void DeleteSekundaerklassifikasjonFromSaksmappe(string saksmappeSystemId, string sekundaerklassifikasjonId)
        {
            var funnet = false;
            if (sekundaerklassifikasjonId != null)
            {
                foreach (var saksmappe in Saksmapper)
                {
                    if (saksmappe.systemID == saksmappeSystemId)
                    {
                        saksmappe.RemoveSekundaerklasseById(sekundaerklassifikasjonId);
                        funnet = true;
                    }
                }
            }
            if (!funnet)
            {
                throw new ArgumentNullException("Saksmappen finnes ikke");
            }
        }

        public static void SetSekundaerklassifikasjonerToSaksmappe(string saksmappeSystemId, KlasseType[] klasseType)
        {
            var funnet = false;
            if (klasseType != null)
            {
                foreach (var saksmappe in Saksmapper)
                {
                    if (saksmappe.systemID == saksmappeSystemId)
                    {
                        saksmappe.sekundaerklassifikasjon = klasseType;
                        funnet = true;
                    }
                }
            }
            if (!funnet)
            {
                throw new ArgumentNullException("Saksmappen finnes ikke");
            }
        }

        public static KlasseType[] GetSekundaerklassifikasjonerBySaksmappeId(string id)
        {
            var saksmappe = GetSaksmappeById(id) ?? throw new ArgumentNullException("Saksmappen finnes ikke");
            return saksmappe.sekundaerklassifikasjon;
        }

    }
}