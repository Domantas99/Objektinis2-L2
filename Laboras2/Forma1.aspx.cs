using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Laboras2
{
    public partial class Forma1 : System.Web.UI.Page
    {  
        readonly string DuomenysPrenumeratoriu = "App_Data/PrenumeratoriaiA.txt";       // Pradinis prenumeratorių duomenų file'as.
        readonly string DuomenysAgentu = "App_Data/AgentaiA.txt";                       // Pradinis agentų duomenų file'as.
        readonly string SpausdinimoFile = "Rezultatai/PradiniaiDuom.txt";               // Pradinių duomenų ir rezultatų spausdinimo file'as.

        int nurodytasMenesis;                                                           // Nurodytas mėnesis pagal kurį reikės atrinkinėti.                                                               
        int kruvis;                                                                     // Minimalus krūvis.
        double Vidurkis = 0;                                                            // Nurodyto mėnesio agentų krūvio vidurkis.

        NodeList<Prenumeratorius> PrenSarasas = new NodeList<Prenumeratorius>();        // Visų prenumeratorių sąrašas.
        NodeList<Prenumeratorius> MenesioPren = new NodeList<Prenumeratorius>();        // Visų to mėnesio prenumeratorių sąrašas.
        NodeList<Agentas> VisiAgentai = new NodeList<Agentas>();                        // Visų agentų sąrašas.
        NodeList<Agentas> MenesioAgentai = new NodeList<Agentas>();                     // Sąrašas atrinktų nurodyto mėnesio agentų.
        NodeList<Agentas> AgentaiAtrinkti = new NodeList<Agentas>();                    // Sąrašas atrinktų nurodyto mėnesio agentų, kurių krūvis daugiau nei vidurkis.                                                                                     
        NodeList<Agentas> AgentaiPasikeite = new NodeList<Agentas>();                   // Sąrašas atrinktų nurodyto mėnesio agentų, kurių krūvis pasikeitė(padidėjo)
                                                                                        // po krūvio paskirstymo.     
 
        /// <summary>
        /// Puslapio užkrovimo komandos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            Label6.Visible = false;
            Label7.Visible = false;
            Label8.Visible = false;
            Label9.Visible = false;
            Table1.Visible = false;
            Table2.Visible = false;
            Table3.Visible = false;
            Table4.Visible = false;
            Button2.Visible = false;

        }
        
        /// <summary>
        /// Mygtuko "Reset" funkcionalumas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            //---------------------------------------------------------------------------------------
            VisiAgentai.NaikintiSarasa();
            PrenSarasas.NaikintiSarasa();
            MenesioAgentai.NaikintiSarasa();
            AgentaiAtrinkti.NaikintiSarasa();
            AgentaiPasikeite.NaikintiSarasa();
            //---------------------------------------------------------------------------------------
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            Label6.Visible = false;
            Label7.Visible = false;
            Label8.Visible = false;
            Label9.Visible = false;
            Table1.Visible = false;
            Table2.Visible = false;
            Table3.Visible = false;
            Table4.Visible = false;
            Table5.Visible = false;
            Button2.Visible = false;
            //---------------------------------------------------------------------------------------
        }

        /// <summary>
        /// Mygtuko "Vykdyti programą!" funkcionalumas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            nurodytasMenesis = int.Parse(TextBox1.Text);
            kruvis = int.Parse(TextBox2.Text);

            if (File.Exists(Server.MapPath(SpausdinimoFile)))
            {
                File.Delete(Server.MapPath(SpausdinimoFile));
            }
            //---------------------------------------------------------------------------------------
            SkaitytiPrenumeratorius(PrenSarasas);
            SpausdintiIFilePrenumeratorius(PrenSarasas, "Pradiniai prenumeratorių duomenys.");
            SkaitytiAgentus(VisiAgentai);
            SpausdintiIFileAgentus(VisiAgentai, "Pradiniai agentų duomenys.");
            RodytiAgentusTable(Table1, VisiAgentai);
            RodytiPrenumeratoriusTable(Table2, PrenSarasas);
            AtrinktiMenesioPrenumeratorius(PrenSarasas, MenesioPren, nurodytasMenesis);
            //---------------------------------------------------------------------------------------
            AgentuKruvioSkaiciavimas(VisiAgentai, MenesioPren, nurodytasMenesis);
            SkaitytiAgentus(MenesioAgentai);
            AgentuKruvioSkaiciavimas(MenesioAgentai, MenesioPren, nurodytasMenesis);
            RodytiAgentusSuPrenumeratoriaisTable(Table5, MenesioAgentai, MenesioPren);
            //SpausdintiIFileAgentus(MenesioAgentai, "Agentai su to mėnesio krūviu:");
            SpausdintiIFileAgentusSuPrenSarasais(MenesioAgentai, MenesioPren, "Mėnesio agentai su jų prenumeratorių sąrašais.");
            //---------------------------------------------------------------------------------------
            AgentaiAtrinkti = AgentaiDaugiauNeiVidurkis(VisiAgentai);
            AgentaiAtrinkti.Rikiuoti();
            SpausdintiIFileAgentus(AgentaiAtrinkti, "Surikiuotas sąrašas atrinktų nurodyto mėnesio agentų, kurių krūvis daugiau nei vidurkis.");    
            Vidurkis = AgentuKruvioVidurkis(VisiAgentai);
            Label8.Text = String.Format("Agentų krūvio vidurkis yra: <b>{0}</b>", Vidurkis);
            //---------------------------------------------------------------------------------------
            Apdoroti(MenesioAgentai, MenesioPren, kruvis, Vidurkis);
            //SpausdintiIFileAgentusSuPrenSarasais(MenesioAgentai, PrenSarasas, "ekperimentas");
            RastiPasikeitusius(VisiAgentai, MenesioAgentai, AgentaiPasikeite);
            //SpausdintiIFileAgentus(AgentaiPasikeite, "Sąrašas atrinktų nurodyto mėnesio agentų, kurių krūvis pasikeitė(padidėjo) po krūvio paskirstymo.");
            SpausdintiIFileAgentusSuPrenSarasais(AgentaiPasikeite, MenesioPren, "Agentų sąrašas, kuriems pasikeitė krūvis, kartu su prenumeratorių sąrašais.");          
            //---------------------------------------------------------------------------------------
            RodytiAgentusTable(Table3, AgentaiAtrinkti);
            RodytiAgentusSuPrenumeratoriaisTable(Table4, AgentaiPasikeite, MenesioPren);

        
            Label1.Visible = true;
            Label2.Visible = true;
            Label3.Visible = true;
            Label6.Visible = true;
            Label7.Visible = true;
            Label8.Visible = true;
            Label9.Visible = true;
            Button2.Visible = true;
            //---------------------------------------------------------------------------------------
        }

        /// <summary>
        /// Funkcija, kuri lentelė parodo agentus su jiems priklausančiais prenumeratoriais.
        /// </summary>
        /// <param name="table">Table</param>
        /// <param name="agentai">Agentų sąrašas</param>
        /// <param name="prenumeratoriai">Prenumeratorių sąrašas</param>
        void RodytiAgentusSuPrenumeratoriaisTable(Table table, NodeList<Agentas> agentai, NodeList<Prenumeratorius> prenumeratoriai)
        {
            //if (agentai.ArYra() == true)          
            {
                for (agentai.Pradzia(); agentai.ArYra();agentai.Sekantis())
                {
                    if (agentai.GautiT().Kruvis > 0)
                    {
                        TableRow row = new TableRow();
                        TableCell cell = new TableCell();
                        //===========================================================
                        cell = new TableCell
                        {
                            Text = "<b>Agento kodas</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Pavarde</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Vardas</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Adresas</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Tel nr.</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Kruvis</b>",
                            ColumnSpan = 2
                        };
                        row.Cells.Add(cell);    
                        //========================================================
                        table.Rows.Add(row);
                        
                        //=======================================================
                        row = new TableRow();
                        cell = new TableCell
                        {
                            Text = agentai.GautiT().AgentoKodas
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = agentai.GautiT().Pavarde
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = agentai.GautiT().Vardas
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = agentai.GautiT().Adresas
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = agentai.GautiT().Telefonas
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = Convert.ToString(agentai.GautiT().Kruvis),
                            ColumnSpan = 2 
                        };
                        row.Cells.Add(cell);
                        //========================================================
                        table.Rows.Add(row);

                        row = new TableRow();
                        //===========================================================
                        cell = new TableCell
                        {
                            Text = "<b>Prenumeratoriaus pavarde</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Adresas</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Laikotarpio pradžia</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Laikotarpio ilgis</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Leidinio kodas</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Leidiniu kiekis</b>"
                        };
                        row.Cells.Add(cell);

                        cell = new TableCell
                        {
                            Text = "<b>Priskirto agento kodas</b>"
                        };
                        row.Cells.Add(cell);
                        //===========================================================
                        table.Rows.Add(row);

                        {
                            for (prenumeratoriai.Pradzia(); prenumeratoriai.ArYra(); prenumeratoriai.Sekantis())
                            {
                                if (prenumeratoriai.GautiT().PriklausantisAgentas == agentai.GautiT().AgentoKodas)
                                {       
                                    //=======================================================
                                    row = new TableRow();
                                    cell = new TableCell
                                    {
                                        Text = prenumeratoriai.GautiT().Pavarde
                                    };
                                    row.Cells.Add(cell);

                                    cell = new TableCell
                                    {
                                        Text = prenumeratoriai.GautiT().Adresas
                                    };
                                    row.Cells.Add(cell);

                                    cell = new TableCell
                                    {
                                        Text = Convert.ToString(prenumeratoriai.GautiT().PradziaL)
                                    };
                                    row.Cells.Add(cell);

                                    cell = new TableCell
                                    {
                                        Text = Convert.ToString(prenumeratoriai.GautiT().TrukmeL)
                                    };
                                    row.Cells.Add(cell);

                                    cell = new TableCell
                                    {
                                        Text = Convert.ToString(prenumeratoriai.GautiT().LeidinioKodas)
                                    };
                                    row.Cells.Add(cell);

                                    cell = new TableCell
                                    {
                                        Text = Convert.ToString(prenumeratoriai.GautiT().LeidiniuKiekis)
                                    };
                                    row.Cells.Add(cell);

                                    cell = new TableCell
                                    {
                                        Text = Convert.ToString(prenumeratoriai.GautiT().PriklausantisAgentas)
                                    };
                                    row.Cells.Add(cell);
                                    //========================================================
                                    table.Rows.Add(row);
                                }
                            }
                        }
                    }
                    table.Visible = true;
                }

            }
        }

        /// <summary>
        /// Funkcija, kuri ant ekrano spausdina prenumetorių sąrašo table'ą.
        /// </summary>
        /// <param name="table">Table'o index'as</param>
        /// <param name="prenumeratoriai">Prenumeratorių sąrašas</param>
        void RodytiPrenumeratoriusTable(Table table, NodeList<Prenumeratorius> prenumeratoriai)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
    
            {
                //===========================================================
                cell = new TableCell
                {
                    Text = "<b>Pavarde</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Adresas</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Laikotarpio pradžia</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Laikotarpio ilgis</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Leidinio kodas</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Leidiniu kiekis</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Agento kodas</b>"
                };
                row.Cells.Add(cell);

                //===========================================================
                table.Rows.Add(row);

                for (prenumeratoriai.Pradzia(); prenumeratoriai.ArYra(); prenumeratoriai.Sekantis())
                {
                    //=======================================================
                    row = new TableRow();
                    cell = new TableCell
                    {
                        Text = prenumeratoriai.GautiT().Pavarde
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = prenumeratoriai.GautiT().Adresas
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = Convert.ToString(prenumeratoriai.GautiT().PradziaL)
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = Convert.ToString(prenumeratoriai.GautiT().TrukmeL)
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = Convert.ToString(prenumeratoriai.GautiT().LeidinioKodas)
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = Convert.ToString(prenumeratoriai.GautiT().LeidiniuKiekis)
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = Convert.ToString(prenumeratoriai.GautiT().PriklausantisAgentas)
                    };
                    row.Cells.Add(cell);
                    //========================================================
                    table.Rows.Add(row);
                }

            }
            table.Visible = true;
        }

        /// <summary>
        /// Funkcija, kuri ant ekrano spausdina agentų sąrašo table'ą.
        /// </summary>
        /// <param name="table">Table'o index'as</param>
        /// <param name="agentai">Agentų sąrašas</param>
        void RodytiAgentusTable(Table table, NodeList<Agentas> agentai)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            
            {             
                //===========================================================
                cell = new TableCell
                {
                    Text = "<b>Kodas</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Pavarde</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Vardas</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Adresas</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Tel nr.</b>"
                };
                row.Cells.Add(cell);

                cell = new TableCell
                {
                    Text = "<b>Kruvis</b>"
                };

                row.Cells.Add(cell);
                //===========================================================
                table.Rows.Add(row);

                for (agentai.Pradzia(); agentai.ArYra(); agentai.Sekantis())
                {
                    //=======================================================
                    row = new TableRow();
                    cell = new TableCell
                    {
                        Text = agentai.GautiT().AgentoKodas
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = agentai.GautiT().Pavarde
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = agentai.GautiT().Vardas
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = agentai.GautiT().Adresas
                    };
                    row.Cells.Add(cell);

                    cell = new TableCell
                    {
                        Text = agentai.GautiT().Telefonas
                    };
                    row.Cells.Add(cell);
                                   
                    cell = new TableCell
                    {
                        Text = Convert.ToString(agentai.GautiT().Kruvis)
                    };
                    row.Cells.Add(cell);
                    //========================================================
                    table.Rows.Add(row);                     
                }
                    table.Visible = true;       
            }

        }

        /// <summary>
        /// Funkcija, kuri randa pasikeitusius agentus, kurių krūvis padidėjo.
        /// </summary>
        /// <param name="VisiAgentai"></param>
        /// <param name="MenesioAgentai"></param>
        /// <param name="AgentaiPasikeite"></param>
        void RastiPasikeitusius(NodeList<Agentas> VisiAgentai, NodeList<Agentas> MenesioAgentai, NodeList<Agentas> AgentaiPasikeite)
        {
            for (VisiAgentai.Pradzia(); VisiAgentai.ArYra(); VisiAgentai.Sekantis())
            {
                for (MenesioAgentai.Pradzia(); MenesioAgentai.ArYra(); MenesioAgentai.Sekantis())
                {                   
                    if (MenesioAgentai.GautiT() == VisiAgentai.GautiT())
                    {
                        if (MenesioAgentai.GautiT().Kruvis > VisiAgentai.GautiT().Kruvis)
                        {
                            AgentaiPasikeite.AddData(MenesioAgentai.GautiT());
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Funkcija, kuri pašalina prenumeratoriaus kodą.
        /// </summary>
        /// <param name="prenumeratoriai">Prenumeratoriai</param>
        /// <param name="agentas">Agentas, pagal kurio kodą pašalins</param>
        void PasalintiAgentoPrenumeratorius(NodeList<Prenumeratorius> prenumeratoriai,Agentas agentas)
        {
            for (prenumeratoriai.Pradzia(); prenumeratoriai.ArYra(); prenumeratoriai.Sekantis())
            {
                if (prenumeratoriai.GautiT().PriklausantisAgentas==agentas.AgentoKodas)
                {
                    prenumeratoriai.GautiT().PriskirtiAgenta("-");
                }
            }

        }

        /// <summary>
        /// Funkcija, kuri pašalina agentus esančius žemiau(arba lygu) nurodyto krūvio ir paskirsto jų
        /// krūvį agentams, kurių krūvis mažiau nei vidurkis.
        /// </summary>
        /// <param name="MenesioAgentai">Agentų sąrašas</param>
        /// <param name="kruvis">Nurodytas minimalus krūvis</param>
        /// <param name="vidurkis">Agentų krūvio vidurkis</param>
        void Apdoroti(NodeList<Agentas> MenesioAgentai, NodeList<Prenumeratorius> pren, int kruvis, double vidurkis)
        {
            int kruvioSuma = 0;

            for (MenesioAgentai.Pradzia(); MenesioAgentai.ArYra(); MenesioAgentai.Sekantis())
            {
                if (MenesioAgentai.GautiT().Kruvis <= kruvis)
                {
                    kruvioSuma += MenesioAgentai.GautiT().Kruvis;
                    PasalintiAgentoPrenumeratorius(pren, MenesioAgentai.GautiT());
                    MenesioAgentai.Delete();                 
                }
            }

            for (int i = 0; i < kruvioSuma; i++)
            {
                for (MenesioAgentai.Pradzia(); MenesioAgentai.ArYra(); MenesioAgentai.Sekantis())
                {
                    if (MenesioAgentai.GautiT().Kruvis < vidurkis)
                    { 
                        for (pren.Pradzia(); pren.ArYra(); pren.Sekantis())
                        {
                            if (pren.GautiT().PriklausantisAgentas == "-")
                            {
                                pren.GautiT().PriskirtiAgenta(MenesioAgentai.GautiT().AgentoKodas);
                                MenesioAgentai.GautiT().AddKruvis();
                                break;
                            }                           
                        }
                        
                    }
                }
            }
         
        }

        /// <summary>
        /// Funkcija, kuri sudaro agentų sąraša, kurių krūvis yra auksčiau nei vidurkio.
        /// </summary>
        /// <param name="agentai">Agentų sąrašas</param>
        /// <returns>Agentų sąrašas</returns>
        NodeList<Agentas> AgentaiDaugiauNeiVidurkis(NodeList<Agentas> agentai)
        {
            NodeList<Agentas> AgentaiDaugiau = new NodeList<Agentas>();
            double vidurkis = AgentuKruvioVidurkis(agentai);

            for (agentai.Pradzia(); agentai.ArYra(); agentai.Sekantis())
            {
                if (agentai.GautiT().Kruvis > vidurkis)
                {
                    AgentaiDaugiau.AddData(agentai.GautiT() as Agentas);
                }
            }

            return AgentaiDaugiau;
        }

        /// <summary>
        /// Funkcija, kuri skaičiuoja agentų krūvio vidurkį.
        /// </summary>
        /// <param name="Agentai">Agentų sąrašas</param>
        /// <returns>Vidurkis</returns>
        double AgentuKruvioVidurkis(NodeList<Agentas> Agentai) // Tą menesį
        {
            int suma = 0;
            int count = 0;
            for (Agentai.Pradzia(); Agentai.ArYra(); Agentai.Sekantis())
            {
                // Jei nulis, tai tada pasirinktą menesį, tas agentas neturi prenumeratorių ir nėra įtaukiamas į krūvio skaičiavimą.
                if (Agentai.GautiT().Kruvis > 0)
                {
                    suma += Agentai.GautiT().Kruvis;
                    count++;
                }
            }
            if (suma == 0)
            {
                return 0;
            }

            return Math.Round(((double)suma / count), 2);
        }

        /// <summary>
        /// Funkcija, kuri atrenka nurodyto mėnesio prenumeratorius
        /// </summary>
        /// <param name="prenumeratoriai">Visi prenumeratoriai</param>
        /// <param name="atrinkti">Atrinkti prenumeratoriai</param>
        /// <param name="nurodytasMenesis">Nurodytas atrinkimo mėnesis</param>
        void AtrinktiMenesioPrenumeratorius(NodeList<Prenumeratorius> prenumeratoriai, NodeList<Prenumeratorius> atrinkti, int nurodytasMenesis)
        {
            for (prenumeratoriai.Pradzia();prenumeratoriai.ArYra();prenumeratoriai.Sekantis())
            {
                if ((nurodytasMenesis >= prenumeratoriai.GautiT().PradziaL &&
                      nurodytasMenesis <= (prenumeratoriai.GautiT().PradziaL + prenumeratoriai.GautiT().TrukmeL)) ||
                    // Jei nurodytas mėnesis yra pvz 1, o prenumeratoriaus laikotrapio pradžia pvz: 12, trukmė 3.
                    (nurodytasMenesis <= prenumeratoriai.GautiT().PradziaL &&
                      ((prenumeratoriai.GautiT().PradziaL + prenumeratoriai.GautiT().TrukmeL) - 12) >= nurodytasMenesis)
                   )
                {
                    atrinkti.AddData(prenumeratoriai.GautiT());
                }
            }

        }

        /// <summary>
        /// Funkcija, kuri skaičiuoja agentų krūvius.
        /// </summary>
        /// <param name="agentai">Agentų sąrašas</param>
        /// <param name="prenumeratoriai">Prenumeratorių sąrašas</param>
        /// <param name="nurodytasMenesis">Nurodytas atrinkimo mėnesis</param>
        void AgentuKruvioSkaiciavimas(NodeList<Agentas> agentai, NodeList<Prenumeratorius> prenumeratoriai, int nurodytasMenesis)
        {        
            for (agentai.Pradzia(); agentai.ArYra(); agentai.Sekantis())
            {
                agentai.GautiT().SetKruvis(0);
                for (prenumeratoriai.Pradzia(); prenumeratoriai.ArYra(); prenumeratoriai.Sekantis())
                {
                    if ((agentai.GautiT().AgentoKodas == prenumeratoriai.GautiT().PriklausantisAgentas &&
                        nurodytasMenesis >= prenumeratoriai.GautiT().PradziaL &&
                        nurodytasMenesis <= (prenumeratoriai.GautiT().PradziaL + prenumeratoriai.GautiT().TrukmeL)) ||
                        // Jei nurodytas mėnesis yra pvz 1, o prenumeratoriaus laikotrapio pradžia pvz: 12, trukmė 3.
                        (agentai.GautiT().AgentoKodas == prenumeratoriai.GautiT().PriklausantisAgentas &&
                        nurodytasMenesis <= prenumeratoriai.GautiT().PradziaL &&
                        ((prenumeratoriai.GautiT().PradziaL + prenumeratoriai.GautiT().TrukmeL) - 12) >= nurodytasMenesis)                     
                        )
                    {
                        agentai.GautiT().AddKruvis();
                    }
                }
            }
            
        }

        /// <summary>
        /// Funkcija, kuri nuskaito prenumeratorių duomenis.
        /// </summary>
        /// <param name="prenumeratoriai"></param>
        void SkaitytiPrenumeratorius(NodeList<Prenumeratorius> prenum)
        {
            using (StreamReader sr = new StreamReader(Server.MapPath(DuomenysPrenumeratoriu), true))
            {
                string pavarde;
                string adresas;
                int lpradzia;
                int ilgisl;
                string leidkodas;
                int lkiekis;
                string agentas;

                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    pavarde = values[0];
                    adresas = values[1];
                    lpradzia = Convert.ToInt32(values[2]);
                    ilgisl = Convert.ToInt32(values[3]);
                    leidkodas = values[4];
                    lkiekis = Convert.ToInt32(values[5]);
                    agentas = values[6];
                    Prenumeratorius p = new Prenumeratorius(pavarde, adresas, lpradzia, ilgisl, leidkodas, lkiekis, agentas);

                    prenum.AddData(p);
                }
            }

        }

        /// <summary>
        /// Funkcija, kuri nuskaito agentų duomenis.
        /// </summary>
        /// <param name="agentai">Agentų sąrašas</param>
        void SkaitytiAgentus(NodeList<Agentas> agentai)
        {
            using (StreamReader sr = new StreamReader(Server.MapPath(DuomenysAgentu), true))
            {
                string agentoKodas;
                string pavarde;
                string vardas;
                string adresas;
                string telefonas;
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    agentoKodas = values[0];
                    pavarde = values[1];
                    vardas = values[2];
                    adresas = values[3];
                    telefonas = values[4];
                    Agentas a = new Agentas(agentoKodas, pavarde, vardas, adresas, telefonas);            
                    agentai.AddData(a);
                }
            }

        }

        /// <summary>
        /// Funkcija, kuri spausdina agentus į file'ą.
        /// </summary>
        /// <param name="agentai">Agentų sąrašas</param>
        /// <param name="text">Tekstas skirtas nusakyti duomenims</param>
        void SpausdintiIFileAgentus(NodeList<Agentas> agentai, string text)
        {
            using (StreamWriter sr = new StreamWriter(Server.MapPath(SpausdinimoFile), true))
            {
                sr.WriteLine("|-----------------------------------------------------------------------------|");
                sr.WriteLine("| " + text);
                sr.WriteLine("|Agento kodas|  Pavardė   |   Vardas   |  Adresas   | Telefonas  |   Krūvis   |");
                sr.WriteLine("|-----------------------------------------------------------------------------|");
                for (agentai.Pradzia(); agentai.ArYra(); agentai.Sekantis())
                {
                    string temp = String.Format("    {0, -7} |" ,agentai.GautiT().Kruvis);
                    if (agentai.GautiT().Kruvis > 0)
                    {
                        sr.WriteLine(agentai.GautiT() + temp);
                        sr.WriteLine("|-----------------------------------------------------------------------------|");
                    }
                    else
                    {
                        sr.WriteLine(agentai.GautiT());
                        sr.WriteLine("|-----------------------------------------------------------------------------|");
                    }
                   
                }
                sr.WriteLine();
                sr.WriteLine();
            }
                        
        }

        /// <summary>
        /// Funkcija, kuri spausdina prenumeratorius į file'ą.
        /// </summary>
        /// <param name="prenumeratoriai">Prenumeratorių sąrašas</param>
        /// <param name="text">Tekstas skirtas nusakyti duomenims</param>
        void SpausdintiIFilePrenumeratorius(NodeList<Prenumeratorius> prenum, string text)
        {
            using (StreamWriter sr = new StreamWriter(Server.MapPath(SpausdinimoFile), true))
            {         
                sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                sr.WriteLine("| " + text);
                sr.WriteLine("|   Pavardė  |     Adresas     | Laikotarpio pradžia | Laikotarpio ilgis | Leidinio kodas | Leidinių kiekis | Agento kodas |");
                sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                for (prenum.Pradzia(); prenum.ArYra(); prenum.Sekantis())
                {
                    sr.WriteLine(prenum.GautiT());
                    sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                }
                sr.WriteLine();
                sr.WriteLine();
            }

        }

        /// <summary>
        /// Funkcija, kuri spausdina agentus su jų prenumeratoriais į file'ą.
        /// </summary>
        /// <param name="agentai">Agentų sąrašas</param>
        /// <param name="prenumertatoriai">Prenumeratorių sąrašas</param>
        void SpausdintiIFileAgentusSuPrenSarasais(NodeList<Agentas> agentai, NodeList<Prenumeratorius> prenumertatoriai, string text)
        {
            using (StreamWriter sr = new StreamWriter(Server.MapPath(SpausdinimoFile), true))
            {
                sr.WriteLine("|-----------------------------------------------------------------------------|");
                sr.WriteLine("| " + text);
                sr.WriteLine("|-----------------------------------------------------------------------------|");
                for (agentai.Pradzia(); agentai.ArYra(); agentai.Sekantis())
                {
                    sr.WriteLine("|Agento kodas|  Pavardė   |   Vardas   |  Adresas   | Telefonas  |   Krūvis   |");
                    sr.WriteLine("|-----------------------------------------------------------------------------|");
                    string temp = String.Format("    {0, -7} |", agentai.GautiT().Kruvis);
                    if (agentai.GautiT().Kruvis > 0)
                    {
                        sr.WriteLine(agentai.GautiT() + temp);
                        sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                        sr.WriteLine("|   Pavardė  |     Adresas     | Laikotarpio pradžia | Laikotarpio ilgis | Leidinio kodas | Leidinių kiekis | Agento kodas |");
                        sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                        for (prenumertatoriai.Pradzia();prenumertatoriai.ArYra();prenumertatoriai.Sekantis())
                        {
                            if (prenumertatoriai.GautiT().PriklausantisAgentas==agentai.GautiT().AgentoKodas)
                            {
                                sr.WriteLine(prenumertatoriai.GautiT());
                            }
                        }

                    }
                    else
                    {
                        sr.WriteLine(agentai.GautiT());
                        sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                        sr.WriteLine("|   Pavardė  |     Adresas     | Laikotarpio pradžia | Laikotarpio ilgis | Leidinio kodas | Leidinių kiekis | Agento kodas |");
                        sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                        for (prenumertatoriai.Pradzia(); prenumertatoriai.ArYra(); prenumertatoriai.Sekantis())
                        {
                            if (prenumertatoriai.GautiT().PriklausantisAgentas == agentai.GautiT().AgentoKodas)
                            {
                                sr.WriteLine(prenumertatoriai.GautiT());
                            }
                        }
                    }
                    sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                    sr.WriteLine("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                    sr.WriteLine("|--------------------------------------------------------------------------------------------------------------------------|");
                }
                sr.WriteLine();
                sr.WriteLine();
            }
        }

    }
}
