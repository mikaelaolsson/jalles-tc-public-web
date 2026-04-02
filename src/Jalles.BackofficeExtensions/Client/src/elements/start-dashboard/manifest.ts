import { ManifestDashboard } from "@umbraco-cms/backoffice/dashboard";

export default [
  {
    type: "dashboard",
    name: "Jalles",
    label: "Jalles",
    alias: "Jalles.Start.Dashboard",
    elementName: "start-dashboard",
    js: () => import("./start-dashboard"),
    weight: 1000,
    meta: {
      label: "Jalles",
      pathname: "start-dashboard"
    },
    conditions: [
      {
        alias: "Umb.Condition.SectionAlias",
        match: "Umb.Section.Content"
      }
    ]
  } as ManifestDashboard
];
