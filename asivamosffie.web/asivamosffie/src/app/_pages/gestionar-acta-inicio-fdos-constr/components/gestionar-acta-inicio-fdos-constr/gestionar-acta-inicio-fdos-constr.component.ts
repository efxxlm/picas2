import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActBeginService } from 'src/app/core/_services/actBegin/act-begin.service';
export interface Contrato {
  fechaAprobacionRequisitos: string;
  numeroContrato:string;
  estado:string;
  enviadoparaInterventor:boolean;
  actaSuscrita:boolean;
}

const ELEMENT_DATA: Contrato[] = [
  {fechaAprobacionRequisitos:"20/06/2020",numeroContrato:"C223456789",estado:"Sin validar",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"21/06/2020",numeroContrato:"C223456790",estado:"Con observaciones",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"22/06/2020",numeroContrato:"C223456791",estado:"Con observaciones",enviadoparaInterventor:true,actaSuscrita:null},
  {fechaAprobacionRequisitos:"26/06/2020",numeroContrato:"C223456794",estado:"Con acta en proceso de firma",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"27/06/2020",numeroContrato:"C223456795",estado:"Con acta suscrita y cargada",enviadoparaInterventor:null,actaSuscrita:true}
];
@Component({
  selector: 'app-gestionar-acta-inicio-fdos-constr',
  templateUrl: './gestionar-acta-inicio-fdos-constr.component.html',
  styleUrls: ['./gestionar-acta-inicio-fdos-constr.component.scss']
})
export class GestionarActaInicioFdosConstrComponent implements OnInit {

  verAyuda = false;
  public rolAsignado;
  public ocpion;
  public selTab;
  contratoObra = false;
  contratoInterventoria = false;
  loadDataItems: Subscription;
  dataTable: any;
  constructor(private services: ActBeginService) { }

  ngOnInit(): void {
    this.loadDataItems = this.services.loadDataItems.subscribe((loadDataItems: any) => {
      if(loadDataItems!=''){
      this.dataTable=loadDataItems;
      }
    }); 
    console.log(this.loadDataItems);
    this.cargarRol();
  }
  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    if (this.rolAsignado == 11) {
      this.ocpion = 1;
    }
    else {
      this.ocpion = 2;
    }
  }
}
