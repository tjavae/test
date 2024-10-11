import { Route, DefaultUrlSerializer, UrlSegment, UrlSegmentGroup } from '@angular/router'

export class LowerCaseUrlSerialize extends DefaultUrlSerializer{
   
    parse(url: string){ 
        url = url.toLocaleLowerCase();
        let segments: UrlSegment[];
        let segmentGroup: UrlSegmentGroup;
        let route: Route;
        let matchSegments = url.split('/');

        let posParams: {[name: string]: UrlSegment} = {};
        let consumed: UrlSegment[] = [];
        let compUrl = "";
        if (matchSegments.length > 1){
            for (let index = 0; index < segments.length; ++index){
                let segment = segments[index].toString().toLowerCase();
                compUrl = compUrl + segment;
            }
            return super.parse(compUrl.toLowerCase());
        } else {
            return super.parse(url.toLowerCase());
        }                           
    }

}